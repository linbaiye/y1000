using DotNetty.Transport.Channels;
using NLog;

namespace y1000.Source.Networking.Connection
{
    public class MessageHandler : SimpleChannelInboundHandler<object>
    {
        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
        
        private IConnectionEventListener? _eventListener;


        public MessageHandler(IConnectionEventListener eventListener)
        {
            _eventListener = eventListener;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            _eventListener?.OnMessageArrived(msg);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            _eventListener = null;
            base.ChannelInactive(context);
        }
    }
}