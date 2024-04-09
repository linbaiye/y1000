using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;
using y1000.Source.Networking;

namespace y1000.code.networking.message
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

        public abstract void Accept(IServerMessageHandler handler);
    }
}