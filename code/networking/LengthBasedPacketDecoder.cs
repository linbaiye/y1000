using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Godot;

namespace y1000.code.networking
{
    public class LengthBasedPacketDecoder : LengthFieldBasedFrameDecoder
    {
        public LengthBasedPacketDecoder() : base(short.MaxValue, 0, 4, 0, 4)
        {

        }

        protected override object Decode(IChannelHandlerContext context, IByteBuffer buffer)
        {
            IByteBuffer input = (IByteBuffer) base.Decode(context, buffer);
            byte[] bytes = new byte[input.ReadableBytes];
            input.ReadBytes(bytes);
            return System.Text.UTF8Encoding.UTF8.GetString(bytes);
        }
    }
}