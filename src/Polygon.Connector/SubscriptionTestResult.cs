using JetBrains.Annotations;

namespace Polygon.Connector
{
    /// <summary>
    ///     ��������� �������� �������� �� ����������
    /// </summary>
    [PublicAPI]
    public sealed class SubscriptionTestResult
    {
        private static readonly SubscriptionTestResult _PassedValue = new SubscriptionTestResult(true, "");
        private static readonly SubscriptionTestResult _FailedValue = new SubscriptionTestResult(false, "");

        private SubscriptionTestResult(bool ok, string message)
        {
            Ok = ok;
            Message = message;
        }

        /// <summary>
        ///     ������ �� ��������
        /// </summary>
        public bool Ok { get; }

        /// <summary>
        ///     ���������
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     ������� ��������, ������������ �����
        /// </summary>
        public static SubscriptionTestResult Passed() => _PassedValue;

        /// <summary>
        ///     ������� ��������, ������������ �����
        /// </summary>
        public static SubscriptionTestResult Passed(string message) => new SubscriptionTestResult(true, message);

        /// <summary>
        ///     ������� ��������, ������������ �������
        /// </summary>
        public static SubscriptionTestResult Failed() => _FailedValue;

        /// <summary>
        ///     ������� ��������, ������������ �������
        /// </summary>
        public static SubscriptionTestResult Failed(string message) => new SubscriptionTestResult(false, message);
    }
}