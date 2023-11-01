using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Godot;

namespace y1000.code.networking
{
    public class PacketHandler : SimpleChannelInboundHandler<string>
    {
        private IPacketHandler packetHandler;
        public PacketHandler(IPacketHandler packetHandler)
        {
            this.packetHandler = packetHandler;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, string msg)
        {
            GD.Print("msg : " + msg);
            packetHandler.Handle(msg);
        }
    }
}