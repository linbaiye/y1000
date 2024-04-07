using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Source.Networking.Protobuf;
using Godot;

namespace y1000.code.networking.message
{
    public class SetPositionMessage : AbstractPositionMessage
    {
        public SetPositionMessage(long id, Vector2I coordinate, Direction direction) : base(id, coordinate, direction)
        {
        }

        public static SetPositionMessage FromPacket(PositionPacket packet)
        {
            return new SetPositionMessage(packet.Id, new Vector2I(packet.X, packet.Y), (Direction)packet.Direction);
        }
        public override string ToString()
        {
            return FormatLog("SetPosition");
        }
    }
}