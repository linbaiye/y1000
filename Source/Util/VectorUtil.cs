using System;
using System.Collections.Generic;
using Godot;
using y1000.Source.Creature;

namespace y1000.Source.Util
{
    public static class VectorUtil
    {

        public const int TileSizeX = 32;
        public const int TileSizeY = 24;
        public static readonly Vector2 TileSize = new(32, 24);


        private static readonly IDictionary<Direction, Vector2> VELOCITY_MAP = new Dictionary<Direction, Vector2>()
        {
            {Direction.RIGHT, new Vector2(TileSizeX, 0)},
            {Direction.DOWN_RIGHT, new Vector2(TileSizeX, TileSizeY)},
            {Direction.DOWN, new Vector2(0, TileSizeY)},
            {Direction.DOWN_LEFT, new Vector2(-TileSizeX, TileSizeY)},
            {Direction.LEFT, new Vector2(-TileSizeX, 0)},
            {Direction.UP_LEFT, new Vector2(-TileSizeX, -TileSizeY)},
            {Direction.UP, new Vector2(0, -TileSizeY)},
            {Direction.UP_RIGHT, new Vector2(TileSizeX, -TileSizeY)},
        };


        public static Vector2 Velocity(Direction direction)
        {
            return VELOCITY_MAP.TryGetValue(direction, out var ret) ? ret : Vector2.Zero;
        }


        public static Vector2I ToCoordinate(this Vector2 vector2)
        {
            return new Vector2I((int)(vector2.X / TileSizeX), (int)(vector2.Y / TileSizeY));
        }

        public static Vector2I Move(this Vector2I src, Direction direction)
        {
            return direction switch
            {
                Direction.UP => new Vector2I(src.X, src.Y - 1),
                Direction.DOWN => new Vector2I(src.X, src.Y + 1),
                Direction.LEFT => new Vector2I(src.X - 1, src.Y),
                Direction.RIGHT => new Vector2I(src.X + 1, src.Y),
                Direction.UP_RIGHT => new Vector2I(src.X + 1, src.Y - 1),
                Direction.DOWN_RIGHT => new Vector2I(src.X + 1, src.Y + 1),
                Direction.DOWN_LEFT => new Vector2I(src.X - 1, src.Y + 1),
                Direction.UP_LEFT => new Vector2I(src.X - 1, src.Y - 1),
                _ => src
            };
        }

        public static int Distance(this Vector2I src, Vector2I dst)
        {
            return Math.Max(Math.Abs(src.X - dst.X), Math.Abs(src.Y - dst.Y));
        }

        
        public static Vector2I Move(this Vector2I src, int x, int y)
        {
            return new Vector2I(src.X + x, src.Y + y);
        }

        public static Vector2 ToPosition(this Vector2I vector2)
        {
            return new Vector2(vector2.X * TileSizeX, vector2.Y * TileSizeY);
        }

        public static Direction GetDirection(this Vector2I src, Vector2I another)
        {
            var p1 = src.ToPosition();
            var p2 = another.ToPosition();
            return (p2 - p1).GetDirection();
        }

        public static Direction GetDirection(this Godot.Vector2 vector)
        {
            var angle = Mathf.Snapped(vector.Angle(), Mathf.Pi / 4) / (Mathf.Pi / 4);
            int dir = Mathf.Wrap((int)angle, 0, 8);
            return dir switch
            {
                0 => Direction.RIGHT,
                1 => Direction.DOWN_RIGHT,
                2 => Direction.DOWN,
                3 => Direction.DOWN_LEFT,
                4 => Direction.LEFT,
                5 => Direction.UP_LEFT,
                6 => Direction.UP,
                7 => Direction.UP_RIGHT,
                _ => Direction.RIGHT,
            };
        }
    }
}