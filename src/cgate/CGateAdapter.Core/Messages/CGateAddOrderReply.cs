using System.Diagnostics;
using CGateAdapter.Messages.FortsMessages;
using JetBrains.Annotations;
using ProtoBuf;

namespace CGateAdapter.Messages
{
    /// <summary>
    ///     ��������� AddOrderReply
    /// </summary>
    [PublicAPI, ProtoContract]
    public sealed class CGateAddOrderReply : CGateMessage
    {
        /// <summary>
        ///     ������� <see cref="CGateAddOrderReply"/> �� <see cref="CgmFortsMsg101"/>
        /// </summary>
        public static CGateAddOrderReply Create(CgmFortsMsg101 reply)
        {
            return new CGateAddOrderReply
            {
                Code = reply.Code,
                Msgid = reply.Msgid,
                OrderId = reply.OrderId,
                UserId = reply.UserId,
                Message = reply.Message,
                StreamRegime = reply.StreamRegime,
                StreamName = reply.StreamName
            };
        }

        /// <summary>
        ///     ������� <see cref="CGateAddOrderReply"/> �� <see cref="CgmFortsMsg109"/>
        /// </summary>
        public static CGateAddOrderReply Create(CgmFortsMsg109 reply)
        {
            return new CGateAddOrderReply
            {
                Code = reply.Code,
                Msgid = reply.Msgid,
                OrderId = reply.OrderId,
                UserId = reply.UserId,
                Message = reply.Message,
                StreamRegime = reply.StreamRegime,
                StreamName = reply.StreamName
            };
        }

        /// <inheritdoc />
        [ProtoIgnore]
        public override string MessageTypeName => "CGateAddOrderReply";

        /// <inheritdoc />
        [ProtoIgnore]
        public override CGateMessageType MessageType => CGateMessageType.CGateAddOrderReply;

        /// <inheritdoc />
        [ProtoIgnore]
        public override CGateStreamType StreamType => CGateStreamType.Preudo;

        /// <summary>
        ///     ���� code
        /// </summary>
        [ProtoMember(101)]
        public int Code { get; set; }

        /// <summary>
        ///     ���� msgid
        /// </summary>
        [ProtoMember(102)]
        public int Msgid { get; set; }

        /// <summary>
        ///     ���� order_id
        /// </summary>
        [ProtoMember(103)]
        public long OrderId { get; set; }

        /// <summary>
        ///     ���� message
        /// </summary>
        [ProtoMember(104)]
        public string Message { get; set; }

        /// <inheritdoc />
        public override void Accept(ICGateMessageVisitor visitor) => visitor.Handle(this);

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override string ToString()
        {
            var builder = new CGateMessageTextBuilder(this);
            builder.Add("code", Code);
            builder.Add("msgid", Msgid);
            builder.Add("order_id", OrderId);
            builder.Add("message", Message);
            return builder.ToString();
        }
    }
}