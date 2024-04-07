using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Godot;
using NLog;
using y1000.code.networking.message;
using y1000.code.util;

namespace y1000.code.networking
{
    public class MessageHandler : SimpleChannelInboundHandler<object>
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IConnectionEventListener _eventListener;

        public MessageHandler(IConnectionEventListener eventListener)
        {
            this._eventListener = eventListener;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            logger.Debug("Received message {0}.", msg);
            _eventListener.OnMessageArrived(msg);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            _eventListener.OnConnectionClosed();
        }
    }
}