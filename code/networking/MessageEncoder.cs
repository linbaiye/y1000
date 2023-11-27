using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Godot;
using Google.Protobuf;
using y1000.code.networking.message;

namespace y1000.code.networking
{
    public class MessageEncoder : MessageToByteEncoder<IGameMessage>
    {
        protected override void Encode(IChannelHandlerContext context, IGameMessage message, IByteBuffer output)
        {
            byte[] data = message.ToPacket().ToByteArray();
            GD.Print("Will send " + data.Length + " bytes.");
            output.WriteBytes(message.ToPacket().ToByteArray());
        }
    }
}