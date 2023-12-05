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
using y1000.code.networking.message;

namespace y1000.code.networking
{
    public class MessageEncoder : MessageToByteEncoder<I2ServerGameMessage>
    {
        protected override void Encode(IChannelHandlerContext context, I2ServerGameMessage message, IByteBuffer output)
        {
            output.WriteBytes(message.ToPacket().ToByteArray());
        }
    }
}