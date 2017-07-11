using System;
using System.Diagnostics;
using JetBrains.Annotations;
using ProtoBuf;

namespace CGateAdapter.Messages
{
    /// <summary>
    ///     ����� ���������� ����� �������� �� ���� ������������� ������� ������ �� ��������� � �� �������� - cgm_orders_log
    /// </summary>
    [PublicAPI, ProtoContract]
    public sealed class CGateOrder : CGateMessage
    {
        /// <summary>
        ///     ������� <see cref="CGateOrder"/> �� <see cref="FutTrades.CgmOrdersLog"/>
        /// </summary>
        public static CGateOrder Create(FutTrades.CgmOrdersLog reply)
        {
            return new CGateOrder
            {
                UserId = reply.UserId,
                ClientCode = reply.ClientCode,
                Comment = reply.Comment,
                ExtId = reply.ExtId,
                IdOrd = reply.IdOrd,
                Action = reply.Action,
                AmountRest = reply.AmountRest,
                Dir = reply.Dir,
                Price = reply.Price,
                Amount = reply.Amount,
                IsinId = reply.IsinId,
                Moment = reply.Moment,
                StreamRegime = reply.StreamRegime,
                StreamName = reply.StreamName
            };
        }

        /// <summary>
        ///     ������� <see cref="CGateOrder"/> �� <see cref="OptTrades.CgmOrdersLog"/>
        /// </summary>
        public static CGateOrder Create(OptTrades.CgmOrdersLog reply)
        {
            return new CGateOrder
            {
                UserId = reply.UserId,
                ClientCode = reply.ClientCode,
                Comment = reply.Comment,
                ExtId = reply.ExtId,
                IdOrd = reply.IdOrd,
                Action = reply.Action,
                AmountRest = reply.AmountRest,
                Dir = reply.Dir,
                Price = reply.Price,
                Amount = reply.Amount,
                IsinId = reply.IsinId,
                Moment = reply.Moment,
                StreamRegime = reply.StreamRegime,
                StreamName = reply.StreamName
            };
        }

        /// <inheritdoc />
        [ProtoIgnore]
        public override string MessageTypeName => "CGateOrder";

        /// <inheritdoc />
        [ProtoIgnore]
        public override CGateMessageType MessageType => CGateMessageType.CGateOrder;

        /// <inheritdoc />
        [ProtoIgnore]
        public override CGateStreamType StreamType => CGateStreamType.Preudo;

        /// <summary>
        ///     ���� moment
        /// </summary>
        [ProtoMember(101)]
        public DateTime Moment { get; set; }

        /// <summary>
        ///     ���� isin_id
        /// </summary>
        [ProtoMember(102)]
        public int IsinId { get; set; }

        /// <summary>
        ///     ���� amount
        /// </summary>
        [ProtoMember(103)]
        public int Amount { get; set; }

        /// <summary>
        ///     ���� price
        /// </summary>
        [ProtoMember(104)]
        public double Price { get; set; }

        /// <summary>
        ///     ���� dir
        /// </summary>
        [ProtoMember(105)]
        public sbyte Dir { get; set; }

        /// <summary>
        ///     ���� amount_rest
        /// </summary>
        [ProtoMember(106)]
        public int AmountRest { get; set; }

        /// <summary>
        ///     ���� action
        /// </summary>
        [ProtoMember(107)]
        public sbyte Action { get; set; }

        /// <summary>
        ///     ���� id_ord
        /// </summary>
        [ProtoMember(108)]
        public long IdOrd { get; set; }

        /// <summary>
        ///     ���� ext_id
        /// </summary>
        [ProtoMember(109)]
        public int ExtId { get; set; }

        /// <summary>
        ///     ���� comment
        /// </summary>
        [ProtoMember(110)]
        public string Comment { get; set; }

        /// <summary>
        ///     ���� client_code
        /// </summary>
        [ProtoMember(111)]
        public string ClientCode { get; set; }

        /// <inheritdoc />
        public override void Accept(ICGateMessageVisitor visitor) => visitor.Handle(this);

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override string ToString()
        {
            var builder = new CGateMessageTextBuilder(this);
            builder.Add("moment", Moment);
            builder.Add("isin_id", IsinId);
            builder.Add("amount", Amount);
            builder.Add("dir", Dir);
            builder.Add("amount_rest", AmountRest);
            builder.Add("action", Action);
            builder.Add("id_ord", IdOrd);
            builder.Add("ext_id", ExtId);
            builder.Add("comment", Comment);
            builder.Add("client_code", ClientCode);

            return builder.ToString();
        }
    }
}