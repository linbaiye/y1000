using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using y1000.code.networking.message;

namespace y1000.code.networking
{
    public class MessageHandler : SimpleChannelInboundHandler<IGameMessage>
    {
        private readonly IConnectionEventListener eventListener;

        public MessageHandler(IConnectionEventListener eventListener)
        {
            this.eventListener = eventListener;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IGameMessage msg)
        {
            eventListener.OnMessageArrived(msg);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            eventListener.OnConnectionClosed();
        }
    }
}