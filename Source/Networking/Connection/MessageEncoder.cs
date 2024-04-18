using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Google.Protobuf;
using NLog;
using y1000.Source.Input;

namespace y1000.Source.Networking.Connection
{
    public class MessageEncoder : MessageToByteEncoder<IClientEvent>
    {
        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
        protected override void Encode(IChannelHandlerContext context, IClientEvent clientEvent, IByteBuffer output)
        {
            LOGGER.Debug("Sending message {0}.", clientEvent);
            output.WriteBytes(clientEvent.ToPacket().ToByteArray());
        }
    }
}