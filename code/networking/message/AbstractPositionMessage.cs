using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.networking.message
{
    public abstract class AbstractPositionMessage : IEntityMessage
    {

        private readonly long _id;

        private readonly Vector2I _coordinate;

        private readonly Direction _direction;

        protected AbstractPositionMessage(long id, Vector2I coordinate, Direction direction)
        {
            _id = id;
            _coordinate = coordinate;
            _direction = direction;
        }

        public Vector2I Coordinate => _coordinate;

        public Direction Direction => _direction;

        public long Id => _id;
    }
}