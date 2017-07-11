using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace CGateAdapter
{
    /// <summary>
    ///     ��������� ��� ������� ��������� ��������� ����������
    /// </summary>
    [PublicAPI]
    public sealed class CGConnectionStateEventArgs : EventArgs
    {
        /// <summary>
        ///     �����������
        /// </summary>
        [DebuggerStepThrough]
        public CGConnectionStateEventArgs(CGConnectionState connectionState)
        {
            ConnectionState = connectionState;
        }

        /// <summary>
        ///     ��������� ����������
        /// </summary>
        public CGConnectionState ConnectionState { get; }
    }
}