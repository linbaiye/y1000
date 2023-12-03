using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Godot;
using y1000.code.networking.message;

namespace y1000.code.networking
{
    public class LengthBasedPacketDecoder : LengthFieldBasedFrameDecoder
    {
        public LengthBasedPacketDecoder() : base(short.MaxValue, 0, 4, 0, 4)
        {

        }

        private IGameMessage DecodeMovementMessage(MovementPacket movementPacket)
        {
            var state = (State)movementPacket.State;
            GD.Print("Received message " + state);
            return state switch
            {
                State.WALK => UpdateMovmentStateMessage.FromPacket(movementPacket),
                State.IDLE => UpdateMovmentStateMessage.FromPacket(movementPacket),
                _ => throw new NotSupportedException(),
            };
        }


        protected override object Decode(IChannelHandlerContext context, IByteBuffer buffer)
        {
            IByteBuffer input = (IByteBuffer)base.Decode(context, buffer);
            byte[] bytes = new byte[input.ReadableBytes];
            input.ReadBytes(bytes);
            Packet packet = Packet.Parser.ParseFrom(bytes);
            return packet.TypedPacketCase switch
            {
                Packet.TypedPacketOneofCase.MovementPacket => DecodeMovementMessage(packet.MovementPacket),
                Packet.TypedPacketOneofCase.LoginPacket => LoginMessage.FromPacket(packet.LoginPacket),
                _ => throw new NotSupportedException()
            };
        }
    }
}