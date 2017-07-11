using System;
using CGateAdapter.Messages;
using JetBrains.Annotations;

namespace CGateAdapter
{
    /// <summary>
    ///     ������� ��� CGate
    /// </summary>
    [PublicAPI]
    public interface ICGAdapter : IDisposable
    {
        /// <summary>
        ///     ��������� ����������
        /// </summary>
        CGConnectionState ConnectionState { get; }

        /// <summary>
        ///     ��������� �������
        /// </summary>
        void Start();

        /// <summary>
        ///     ���������� �������
        /// </summary>
        void Stop();

        /// <summary>
        ///     ��������� ���������
        /// </summary>
        void SendMessage([NotNull] CGateMessage message);

        /// <summary>
        ///     ������� ��������� ��������� ����������
        /// </summary>
        event EventHandler<CGConnectionStateEventArgs> ConnectionStateChanged;

        /// <summary>
        ///     ������� ��������� ������ ���������� ��������� �������� ������
        /// </summary>
        event EventHandler<CGateMessageEventArgs> MarketdataMessageReceived;

        /// <summary>
        /// ������� ��������� ������ ���������� ���������, ���������� � ����������� ������
        /// </summary>
        event EventHandler<CGateMessageEventArgs> ExecutionMessageReceived;
    }
}