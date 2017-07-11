using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CGateAdapter
{
    /// <summary>
    ///     ����� ������
    /// </summary>
    [PublicAPI]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum StreamRegime
    {
        /// <summary>
        ///     �������
        /// </summary>
        SNAPSHOT,

        /// <summary>
        ///     ������-�����
        /// </summary>
        ONLINE,
        
        /// <summary>
        ///     ����� ������
        /// </summary>
        CLOSED
    }
}