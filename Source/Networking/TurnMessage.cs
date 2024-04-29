using Godot;
using Source.Networking.Protobuf;
using y1000.code;
using y1000.code.networking.message;
using y1000.Source.Creature;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking
{
    public class TurnMessage : AbstractPositionMessage
    {
        private TurnMessage(long id, Vector2I coordinate, Direction direction) : base(id, coordinate, direction)
        {
        }

        public static TurnMessage FromPacket(PositionPacket packet)
        {
            return new TurnMessage(packet.Id, new Vector2I(packet.X, packet.Y), (Direction)packet.Direction);
        }

        public override void Accept(IServerMessageVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return FormatLog("Turn");
        }
    }
}