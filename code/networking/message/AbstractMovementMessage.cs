using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Code.Networking.Gen;
using Godot;

namespace y1000.code.networking.message
{
    public abstract class AbstractMovementMessage : IGameMessage
    {
        private readonly int id;

        private readonly long t;

        public AbstractMovementMessage(int _id)
        {
            id = _id;
            t = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public AbstractMovementMessage(int _id, Direction _dir, Point _point, long timestamp) : this(_id) {
            Direction = _dir;
            Coordinate = _point;
            t = timestamp;
        }

        public Point Coordinate { get; set; }

        public Direction Direction { get; set; }

        public int Id => id;

        public long Timestamp => t;
    }
}