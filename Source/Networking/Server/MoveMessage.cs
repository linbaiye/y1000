using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Creature;

namespace y1000.Source.Networking.Server
{
    public class MoveMessage : AbstractPositionMessage
    {
        private MoveMessage(long id, Vector2I coor, Direction direction, CreatureState state) : base(id, coor, direction, state) {}


        public static MoveMessage FromPacket(PositionPacket positionPacket)
        {
            return new MoveMessage(positionPacket.Id, new Vector2I(positionPacket.X, positionPacket.Y), (Direction)positionPacket.Direction, (CreatureState)positionPacket.State);
        }

        public override string ToString()
        {
            return FormatLog("Move");
        }

        public override void Accept(IServerMessageVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}