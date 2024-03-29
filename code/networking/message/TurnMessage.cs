using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;
using Godot;

namespace y1000.code.networking.message
{
    public class TurnMessage : AbstractPositionMessage
    {
        public TurnMessage(long id, Vector2I coordinate, Direction direction) : base(id, coordinate, direction)
        {
        }

        public static TurnMessage FromPacket(PositionPacket packet)
        {
            return new TurnMessage(packet.Id, new Vector2I(packet.X, packet.Y), (Direction)packet.Direction);
        }
    }
}