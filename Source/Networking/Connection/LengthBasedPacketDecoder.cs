using System;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using NLog;
using Source.Networking.Protobuf;


namespace y1000.Source.Networking.Connection
{
    public class LengthBasedPacketDecoder : LengthFieldBasedFrameDecoder
    {
        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

        private MessageFactory? _messageFactory;

        public LengthBasedPacketDecoder(MessageFactory messageFactory) : base(short.MaxValue, 0, 4, 0, 4)
        {
            _messageFactory = messageFactory;
        }

        public override void ChannelInactive(IChannelHandlerContext ctx)
        {
            _messageFactory = null;
            base.ChannelInactive(ctx);
        }


        protected override object? Decode(IChannelHandlerContext context, IByteBuffer buffer)
        {
            if (_messageFactory == null)
                return null;
            try
            {
                IByteBuffer input = (IByteBuffer)base.Decode(context, buffer);
                if (input == null)
                {
                    return null;
                }
                byte[] bytes = new byte[input.ReadableBytes];
                input.ReadBytes(bytes);
                Packet packet = Packet.Parser.ParseFrom(bytes);
                input.Release();
                var msg = _messageFactory.Create(packet);
                return msg;
            }
            catch (Exception e)
            {
                LOGGER.Error(e, "Failed to decode message.");
                throw new NotImplementedException();
            }
        }
    }
}