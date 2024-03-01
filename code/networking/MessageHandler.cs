using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Godot;
using y1000.code.networking.message;
using y1000.code.util;

namespace y1000.code.networking
{
    public class MessageHandler : SimpleChannelInboundHandler<object>
    {
        private readonly IConnectionEventListener eventListener;

        public MessageHandler(IConnectionEventListener eventListener)
        {
            this.eventListener = eventListener;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            LOG.Debug("Received message " + msg);
            eventListener.OnMessageArrived(msg);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            eventListener.OnConnectionClosed();
        }
    }
}