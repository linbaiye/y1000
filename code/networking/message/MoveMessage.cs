using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Source.Networking.Protobuf;
using Godot;
using y1000.code.util;
using y1000.Source.Creature;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.code.networking.message
{
    public class MoveMessage : AbstractPositionMessage
    {
        private MoveMessage(long id, Vector2I coor, Direction direction) : base(id, coor, direction) {}


        public static MoveMessage FromPacket(PositionPacket positionPacket)
        {
            return new MoveMessage(positionPacket.Id, new Vector2I(positionPacket.X, positionPacket.Y), (Direction)positionPacket.Direction);
        }

        public override string ToString()
        {
            return FormatLog("Move");
        }

        public override void HandleBy(IServerMessageHandler handler)
        {
            handler.Handle(this);
        }
    }
}