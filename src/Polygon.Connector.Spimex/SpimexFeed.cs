﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGlobal.DeadlockDetection;
using Polygon.Messages;
using SpimexAdapter;
using SpimexAdapter.FTE;

namespace Polygon.Connector.Spimex
{
    internal class SpimexFeed : Feed, IInstrumentParamsSubscriber, IOrderBookSubscriber, IInstrumentTickerLookup, ISubscriptionTester<SpimexInstrumentData>
    {
        private readonly SpimexConnector connector;
        private readonly InfoCommClient infoClient;

        private readonly IDictionary<string, InfoSecurity> securitiesMap = new Dictionary<string, InfoSecurity>();
        private readonly IDictionary<string, InstrumentParams> instrumentsParams = new Dictionary<string, InstrumentParams>();

        private readonly IRwLockObject allInstrumentLock = DeadlockMonitor.ReaderWriterLock<SpimexFeed>("allInstrumentLock");

        private readonly ISet<string> allInstruments = new HashSet<string>();
        private readonly ISet<string> subscribedInstruments = new HashSet<string>();
        private readonly ISet<string> subscribedBooks = new HashSet<string>();
        
        public SpimexFeed(SpimexConnector connector, InfoCommClient infoClient)
        {
            this.connector = connector;
            this.infoClient = infoClient;

            infoClient.OnInfoSecurity += InfoClient_OnInfoSecurity;
            infoClient.OnInfoSecboard += InfoClient_OnInfoSecboard;

            infoClient.OnInfoOrder += InfoClient_OnInfoOrder;

            infoClient.OnError += InfoClient_OnError;
        }

        public override void Start() { }

        public override void Stop() { }

        #region IInstrumentParamsSubscriber
        
        public async Task<SubscriptionResult> Subscribe(Instrument instrument)
        {
            var code = await connector.ResolveInstrumentAsync(instrument);
            if (code == null)
            {
                return SubscriptionResult.Error(instrument, "Unable to resolve instrument symbol");
            }
            
            using (allInstrumentLock.WriteLock())
            {
                if (allInstruments.Contains(code))
                {
                    if (!subscribedInstruments.Contains(code))
                    {
                        subscribedInstruments.Add(code);

                        InstrumentParams ip;
                        if (instrumentsParams.TryGetValue(code, out ip))
                        {
                            OnMessageReceived(ip);
                        }
                        else
                        {
                            ip =new InstrumentParams { Instrument = instrument };
                            instrumentsParams[code] = ip;
                        }
                        
                        return SubscriptionResult.OK(instrument);
                    }

                    return SubscriptionResult.Error(instrument, "Already subscribed.");
                }
            }

            return SubscriptionResult.Error(instrument, "Instrument doesn't exist.");
        }

        public void Unsubscribe(Instrument instrument)
        {
            var code = connector.ResolveInstrumentAsync(instrument).Result;
            if (code == null)
            {
                return;
            }
            
            if (subscribedInstruments.Contains(code))
            {
                subscribedInstruments.Remove(code);
            }
        }

        #endregion

        #region IOrderBookSubscriber

        public void SubscribeOrderBook(Instrument instrument)
        {
            var code = connector.ResolveInstrumentAsync(instrument).Result;
            if (code == null)
            {
                return;
            }
            using (allInstrumentLock.WriteLock())
            {
                if (allInstruments.Contains(code))
                {
                    subscribedBooks.Add(code);
                    
                    if (buildersMap.TryGetValue(code, out var builder))
                    {
                        OnMessageReceived(builder.BuildBook());
                    }
                    else
                    {
                        builder = new OrderBookBuilder(instrument);
                        buildersMap[code] = builder;
                    }
                }
            }
        }

        public void UnsubscribeOrderBook(Instrument instrument)
        {
            var code = connector.ResolveInstrumentAsync(instrument).Result;
            if (code == null)
            {
                return;
            }

            subscribedBooks.Remove(code);
        }

        #endregion

        #region Подписки

        #region Параметры

        private void InfoClient_OnInfoSecurity(InfoSecurity security)
        {
            securitiesMap[security.code] = security;
        }

        private void InfoClient_OnInfoSecboard(InfoSecboard secboard)
        {
            using (allInstrumentLock.WriteLock())
            {
                var code = secboard.code;
                InstrumentParams ip;
                if (!instrumentsParams.TryGetValue(code, out ip))
                {
                    allInstruments.Add(code);

                    ip = CreateParams(secboard);
                    if (ip == null)
                    {
                        return;
                    }

                    instrumentsParams[code] = ip;
                }

                ip.TopPriceLimit = PriceHelper.ToPrice(secboard.price_max);
                ip.BottomPriceLimit = PriceHelper.ToPrice(secboard.price_min);

                ip.Settlement = PriceHelper.ToPrice(secboard.market_price);
                ip.PreviousSettlement = PriceHelper.ToPrice(secboard.prev_market_price);
                ip.LastPrice = PriceHelper.ToPrice(secboard.last_trade_price);

                //ip.SessionEndTime = secboard.finish_trade_time;

                ip.BestBidPrice = PriceHelper.ToPrice(secboard.best_buy);
                ip.BestOfferPrice = PriceHelper.ToPrice(secboard.best_sell);

                if (subscribedInstruments.Contains(code))
                {
                    OnMessageReceived(ip);
                }
            }
        }

        private InstrumentParams CreateParams(InfoSecboard secboard)
        {
            var instrument = connector.ResolveSymbolAsync(secboard.code).Result;
            if (instrument == null)
            {
                return null;
            }

            var security = securitiesMap[secboard.security];

            return new InstrumentParams
            {
                Instrument = instrument,
                //ExpirationDate = security.execution_day,
                PriceStep = PriceHelper.ToPrice(security.price_step),
                LotSize = security.lot_size
            };
        }


        #endregion

        #region Стаканы

        private readonly IDictionary<string, OrderBookBuilder> buildersMap = new Dictionary<string, OrderBookBuilder>();

        //private readonly Dictionary<string, ShortOrderInfo> ordersByCode = new Dictionary<string, ShortOrderInfo>();
        //private readonly Dictionary<string, SortedList<decimal, int>> ordersBySecurityAndPrice = new Dictionary<string, SortedList<decimal, int>>();

        //private sealed class ShortOrderInfo
        //{
        //    public string Security { get; set; }

        //    public BuySell Operation { get; set; }
        //    public long Quantity { get; set; }
        //    public decimal Price { get; set; }
        //}

        private void InfoClient_OnInfoOrder(InfoOrder order)
        {
            var security = order.security + "|" + order.board;

            if (string.IsNullOrEmpty(security))
            {
                //Logger.DebugFormat("Получено обновление заявки с пустым полем security. OrderId: {0}", order.code);
                return;
            }
            
            if (!buildersMap.TryGetValue(security, out var builder))
            {
                var instrument = connector.ResolveSymbolAsync(security).Result;
                if (instrument == null)
                {
                    return;
                }

                builder = new OrderBookBuilder(instrument);
                buildersMap[security] = builder;
            }

            if (!builder.ProcessOrder(order))
            {
                return;
            }

            if (subscribedBooks.Contains(security))
            {
                OnMessageReceived(builder.BuildBook());
                //TODO fire
            }
        }

        #endregion

        private void InfoClient_OnError(int code, string message)
        {
            if (SendErrorMessages)
            {
                OnMessageReceived(new ErrorMessage { Message = $"Code = {code}; Message = {message}" });
            }
        }

        #endregion

        #region Поиск инструментов по коду

        /// <summary>
        ///     Поиск тикеров по (частичному) коду
        /// </summary>
        public string[] LookupInstruments(string code, int maxResults)
        {
            string[] results;
            using (allInstrumentLock.ReadLock())
            {
                results = allInstruments
                    .Where(_ => _.IndexOf(code, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToArray();
            }

            var orderedResults = results
                .OrderBy(result =>
                {
                    var distance = LevenshteinDistance(result, code);
                    var relevance = 1d / (1d + distance);
                    return relevance;
                })
                .Take(maxResults)
                .ToArray();
            return orderedResults;

            // Расчет расстояния Левенштейна для пары строк
            // См. http://stackoverflow.com/a/6944095
            int LevenshteinDistance(string s, string t)
            {
                if (string.IsNullOrEmpty(s))
                {
                    if (string.IsNullOrEmpty(t))
                    {
                        return 0;
                    }

                    return t.Length;
                }

                if (string.IsNullOrEmpty(t))
                {
                    return s.Length;
                }

                var n = s.Length;
                var m = t.Length;
                var d = new int[n + 1, m + 1];

                // initialize the top and right of the table to 0, 1, 2, ...
                for (var i = 0; i <= n; d[i, 0] = i++) { }
                for (var j = 1; j <= m; d[0, j] = j++) { }

                for (var i = 1; i <= n; i++)

                {
                    for (var j = 1; j <= m; j++)
                    {
                        var cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                        var min1 = d[i - 1, j] + 1;
                        var min2 = d[i, j - 1] + 1;
                        var min3 = d[i - 1, j - 1] + cost;
                        d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                    }
                }

                return d[n, m];
            }
        }
        
        #endregion

        public override void Dispose()
        {
            infoClient.OnInfoSecurity -= InfoClient_OnInfoSecurity;
            infoClient.OnInfoSecboard -= InfoClient_OnInfoSecboard;

            infoClient.OnInfoOrder -= InfoClient_OnInfoOrder;

            infoClient.OnError -= InfoClient_OnError;
        }

        #region ISubscriptionTester

        /// <summary>
        ///     Проверить подписку 
        /// </summary>
        Task<SubscriptionTestResult> ISubscriptionTester<SpimexInstrumentData>.TestSubscriptionAsync(SpimexInstrumentData data)
        {
            using (allInstrumentLock.WriteLock())
            {
                if (!allInstruments.Contains(data.Symbol))
                {
                    return Task.FromResult(SubscriptionTestResult.Failed());
                }

                return Task.FromResult(SubscriptionTestResult.Passed());
            }
        }

        #endregion
    }
}
