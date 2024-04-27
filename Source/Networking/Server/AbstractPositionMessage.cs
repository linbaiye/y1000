using Godot;
using y1000.code;
using y1000.code.networking.message;
using y1000.Source.Creature;

namespace y1000.Source.Networking.Server
{
    public abstract class AbstractPositionMessage : IEntityMessage
    {
        protected AbstractPositionMessage(long id, Vector2I coordinate, Direction direction)
        {
            Id = id;
            Coordinate = coordinate;
            Direction = direction;
        }

        public Vector2I Coordinate { get; }

        public Direction Direction { get; }

        public long Id { get; }


        protected string FormatLog(string type)
        {
            return "[Id:" + Id + ", Type: " + type + ", Coordinate: " + Coordinate + ", Dir:" + Direction + "]";
        }

        public abstract void HandleBy(IServerMessageHandler handler);
    }
}