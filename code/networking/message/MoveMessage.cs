using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Source.Networking.Protobuf;
using Godot;
using y1000.code.util;

namespace y1000.code.networking.message
{
    public class MoveMessage : AbstractPositionMessage
    {
        public MoveMessage(long _id, Vector2I coor, Direction direction) : base(_id, coor, direction) {}


        public static MoveMessage FromPacket(PositionPacket positionPacket)
        {
            return new MoveMessage(positionPacket.Id, new Vector2I(positionPacket.X, positionPacket.Y), (Direction)positionPacket.Direction);
        }

        public override string ToString()
        {
            return FormatLog("Move");
        }
    }
}