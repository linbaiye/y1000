using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Godot;
using Google.Protobuf.WellKnownTypes;
using y1000.code.networking.message;
using y1000.code.player;
using y1000.code.player.snapshot;
using y1000.code.util;

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
            return state switch
            {
                State.WALK => UpdateMovmentStateMessage.FromPacket(movementPacket),
                State.IDLE => UpdateMovmentStateMessage.FromPacket(movementPacket),
                _ => throw new NotSupportedException(),
            };
        }


        private IInterpolation DecodeInterpolation(InterpolationPacket packet)
        {
            switch ((State)packet.State)
            {
                case State.IDLE:
                    return IdleInterpolation.Parse(packet);
                default:
                    throw new NotSupportedException();
            }
        }


        private IGameMessage DecodeInterpolations(InterpolationsPacket interpolationsPacket)
        {
            List<IInterpolation> result = new List<IInterpolation>();
            foreach (var packet in interpolationsPacket.Interpolations)
            {
                result.Add(DecodeInterpolation(packet));
            }
            return new InterpolationsMessage(result);
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
                Packet.TypedPacketOneofCase.Interpolations => DecodeInterpolations(packet.Interpolations),
                _ => throw new NotSupportedException()
            };
        }
    }
}