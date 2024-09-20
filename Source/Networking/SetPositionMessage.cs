using Godot;
using NLog;
using Source.Networking.Protobuf;
using y1000.Source.Creature;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking
{
    public class SetPositionMessage : AbstractPositionMessage
    {
        private SetPositionMessage(long id, Vector2I coordinate, Direction direction, CreatureState state) : base(id, coordinate, direction, state)
        {
        }
        
        public static SetPositionMessage FromPacket(PositionPacket packet)
        {
            return new SetPositionMessage(packet.Id, new Vector2I(packet.X, packet.Y), (Direction)packet.Direction, (CreatureState)packet.State);
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