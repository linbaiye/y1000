using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace y1000.code.networking
{
    public class MessageDecoder : LengthFieldBasedFrameDecoder
    {
        public MessageDecoder() : base(short.MaxValue, 0, 4, 0, 4) {}

        protected override object Decode(IChannelHandlerContext context, IByteBuffer input)
        {
            IByteBuffer byteBuf = (IByteBuffer)base.Decode(context, input);
            byte[] bytes = new byte[byteBuf.ReadableBytes];
            byteBuf.ReadBytes(bytes);
            return Packet.Parser.ParseFrom(bytes);
        }
    }
}