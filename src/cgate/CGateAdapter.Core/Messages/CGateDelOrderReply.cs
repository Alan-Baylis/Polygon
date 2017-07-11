using System.Diagnostics;
using CGateAdapter.Messages.FortsMessages;
using JetBrains.Annotations;
using ProtoBuf;

namespace CGateAdapter.Messages
{
    /// <summary>
    ///     ����� ���������� ����� �������� �� ���� ������������� ������� - 
    ///     ������� �� �������� ���������� �� ������ ������ �� ��������� � �� �������� - cgm_FORTS_MSG102 � cgm_FORTS_MSG110.
    /// </summary>
    [PublicAPI, ProtoContract]
    public sealed class CGateDelOrderReply : CGateMessage
    {
        /// <summary>
        ///     ������� <see cref="CGateDelOrderReply"/> �� <see cref="CgmFortsMsg102"/>
        /// </summary>
        public static CGateDelOrderReply Create(CgmFortsMsg102 reply)
        {
            return new CGateDelOrderReply
            {
                UserId = reply.UserId,
                Code = reply.Code,
                Message = reply.Message,
                Amount = reply.Amount,
                StreamRegime = reply.StreamRegime,
                StreamName = reply.StreamName
            };
        }

        /// <summary>
        ///     ������� <see cref="CGateDelOrderReply"/> �� <see cref="CgmFortsMsg110"/>
        /// </summary>
        public static CGateDelOrderReply Create(CgmFortsMsg110 reply)
        {
            return new CGateDelOrderReply
            {
                UserId = reply.UserId,
                Code = reply.Code,
                Message = reply.Message,
                Amount = reply.Amount,
                StreamRegime = reply.StreamRegime,
                StreamName = reply.StreamName
            };
        }

        /// <inheritdoc />
        [ProtoIgnore]
        public override string MessageTypeName => "CGateDelOrderReply";

        /// <inheritdoc />
        [ProtoIgnore]
        public override CGateMessageType MessageType => CGateMessageType.CGateDelOrderReply;

        /// <inheritdoc />
        [ProtoIgnore]
        public override CGateStreamType StreamType => CGateStreamType.Preudo;

        /// <summary>
        ///     ���� amount
        /// </summary>
        [ProtoMember(101)]
        public int Amount { get; set; }

        /// <summary>
        ///     ���� message
        /// </summary>
        [ProtoMember(102)]
        public string Message { get; set; }

        /// <summary>
        ///     ���� code
        /// </summary>
        [ProtoMember(103)]
        public int Code { get; set; }

        /// <inheritdoc />
        public override void Accept(ICGateMessageVisitor visitor) => visitor.Handle(this);

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override string ToString()
        {
            var builder = new CGateMessageTextBuilder(this);
            builder.Add("amount", Amount);
            builder.Add("message", Message);
            builder.Add("code", Code);

            return builder.ToString();
        }
    }
}