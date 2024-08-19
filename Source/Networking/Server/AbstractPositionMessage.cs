using Godot;
using y1000.Source.Creature;

namespace y1000.Source.Networking.Server
{
    public abstract class AbstractPositionMessage : IEntityMessage
    {
        protected AbstractPositionMessage(long id, Vector2I coordinate, Direction direction, CreatureState state)
        {
            Id = id;
            Coordinate = coordinate;
            Direction = direction;
            State = state;
        }

        public Vector2I Coordinate { get; }

        public Direction Direction { get; }
        
        public CreatureState State { get; }

        public long Id { get; }


        protected string FormatLog(string type)
        {
            return "[Id:" + Id + ", Type: " + type + ", Coordinate: " + Coordinate + ", Dir:" + Direction + ", State:" + State + "]";
        }

        public abstract void Accept(IServerMessageVisitor visitor);
    }
}