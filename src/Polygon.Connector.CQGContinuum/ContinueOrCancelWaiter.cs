using System.Threading;
using JetBrains.Annotations;

namespace Polygon.Connector.CQGContinuum
{
    /// <summary>
    /// ������� ��� �������� ������ ��� ����������� ������
    /// </summary>
    internal class ContinueOrCancelWaiter
    {
        #region fields

        /// <summary>
        /// ����� ������ ������
        /// </summary>
        private CancellationToken cancellationToken;

        /// <summary>
        /// ����� ����������� ������
        /// </summary>
        private readonly WaitHandle continuationHandle;

        #endregion

        #region .ctor

        public ContinueOrCancelWaiter(CancellationToken cancellationToken, [NotNull] WaitHandle continuationHandle)
        {
            this.cancellationToken = cancellationToken;
            this.continuationHandle = continuationHandle;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// True ���� ������ ����� ����������, false ���� ��������� cancellationToken � ������ ����� ���������
        /// </summary>
        /// <returns></returns>
        public bool Wait()
        {
            var waited = new[] { cancellationToken.WaitHandle, continuationHandle };
            return WaitHandle.WaitAny(waited) != 0;
        }

        #endregion
    }
}