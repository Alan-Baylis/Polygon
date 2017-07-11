namespace CGateAdapter
{
    /// <summary>
    ///     ��������� ����������
    /// </summary>
    public enum CGConnectionState
    {
        /// <summary>
        ///     ��� ����������, ������� ������� �����������
        /// </summary>
        Shutdown,

        /// <summary>
        ///     ��� ����������, ������� �� ������� �����������
        /// </summary>
        Disconnected,

        /// <summary>
        ///     ���� ��������� ����������
        /// </summary>
        Connecting,

        /// <summary>
        ///     ���������� �����������
        /// </summary>
        Connected
    }
}