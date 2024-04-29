using Godot;
using Source.Networking.Protobuf;
using y1000.code;
using y1000.code.networking.message;
using y1000.Source.Creature;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking
{
    public class SetPositionMessage : AbstractPositionMessage
    {
        private SetPositionMessage(long id, Vector2I coordinate, Direction direction) : base(id, coordinate, direction)
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

        public override void Accept(IServerMessageVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}