using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Godot;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using NLog;
using y1000.code.networking.message;
using y1000.Source.Input;

namespace y1000.code.networking
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