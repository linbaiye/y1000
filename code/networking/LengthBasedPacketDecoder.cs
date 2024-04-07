using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Source.Networking.Protobuf;
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

        private AbstractPositionMessage DecodePositionMessage(PositionPacket positionPacket)
        {
            return (PositionType)positionPacket.Type switch
            {
                PositionType.MOVE => MoveMessage.FromPacket(positionPacket),
                PositionType.TURN => TurnMessage.FromPacket(positionPacket),
                PositionType.SET => SetPositionMessage.FromPacket(positionPacket),
                _ => throw new NotSupportedException(),
            };
        }


        private IInterpolation DecodeInterpolation(InterpolationPacket packet)
        {
            switch ((CreatureState)packet.State)
            {
                case CreatureState.IDLE:
                    return IdleInterpolation.Parse(packet);
                default:
                    throw new NotSupportedException();
            }
        }


        private IEntityMessage DecodeInterpolations(InterpolationsPacket interpolationsPacket)
        {
            List<IInterpolation> result = new List<IInterpolation>();
            foreach (var packet in interpolationsPacket.Interpolations)
            {
                result.Add(DecodeInterpolation(packet));
            }
            return new InterpolationsMessage(result);
        }

        private InputResponseMessage DecodeInputRespoinse(InputResponsePacket packet)
        {
            AbstractPositionMessage positionMessage = DecodePositionMessage(packet.PositionPacket);
            return new InputResponseMessage(packet.Sequence, positionMessage);
        }


        protected override object Decode(IChannelHandlerContext context, IByteBuffer buffer)
        {
            IByteBuffer input = (IByteBuffer)base.Decode(context, buffer);
            byte[] bytes = new byte[input.ReadableBytes];
            input.ReadBytes(bytes);
            Packet packet = Packet.Parser.ParseFrom(bytes);
            return packet.TypedPacketCase switch
            {
                Packet.TypedPacketOneofCase.PositionPacket => DecodePositionMessage(packet.PositionPacket),
                Packet.TypedPacketOneofCase.LoginPacket => LoginMessage.FromPacket(packet.LoginPacket),
                Packet.TypedPacketOneofCase.ResponsePacket => DecodeInputRespoinse(packet.ResponsePacket),
                Packet.TypedPacketOneofCase.Interpolations => DecodeInterpolations(packet.Interpolations),
                _ => throw new NotSupportedException()
            };
        }
    }
}