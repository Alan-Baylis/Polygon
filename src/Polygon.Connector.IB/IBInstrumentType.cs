using JetBrains.Annotations;
using Polygon.Diagnostics;

namespace Polygon.Connector.InteractiveBrokers
{
    /// <summary>
    ///     ��� ����������� (������������ � ����������� IB � IQFeed)
    /// </summary>
    [PublicAPI]
    public enum IBInstrumentType
    {
        /// <summary>
        ///     �������������� ��������
        /// </summary>
        [EnumMemberName("UNDEF")]
        Unknown,

        /// <summary>
        ///     �����
        /// </summary>
        [EnumMemberName("EQ")]
        Equity,

        /// <summary>
        ///     ������
        /// </summary>
        [EnumMemberName("IDX")]
        Index,

        /// <summary>
        ///     ���������
        /// </summary>
        [EnumMemberName("CMDT")]
        Commodity,

        /// <summary>
        ///     ������
        /// </summary>
        [EnumMemberName("FX")]
        FX,

        /// <summary>
        ///     �������
        /// </summary>
        [EnumMemberName("FUTURE")]
        Future,

        /// <summary>
        ///     ������ �� �����
        /// </summary>
        [EnumMemberName("ASSET_OPTION")]
        AssetOption,

        /// <summary>
        ///     ������ �� �������
        /// </summary>
        [EnumMemberName("FUTURE_OPTION")]
        FutureOption
    }
}