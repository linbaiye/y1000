using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;

namespace y1000.code
{
    public static class VectorUtil
    {

        public const int TILE_SIZE_X = 32;
        public const int TILE_SIZE_Y = 24;
        public static readonly Vector2 TILE_SIZE = new(32, 24);


        private static readonly IDictionary<Direction, Vector2> VELOCITY_MAP = new Dictionary<Direction, Vector2>()
        {
            {Direction.RIGHT, new Vector2(TILE_SIZE_X, 0)},
            {Direction.DOWN_RIGHT, new Vector2(TILE_SIZE_X, TILE_SIZE_Y)},
            {Direction.DOWN, new Vector2(0, TILE_SIZE_Y)},
            {Direction.DOWN_LEFT, new Vector2(-TILE_SIZE_X, TILE_SIZE_Y)},
            {Direction.LEFT, new Vector2(-TILE_SIZE_X, 0)},
            {Direction.UP_LEFT, new Vector2(-TILE_SIZE_X, -TILE_SIZE_Y)},
            {Direction.UP, new Vector2(0, -TILE_SIZE_Y)},
            {Direction.UP_RIGHT, new Vector2(TILE_SIZE_X, -TILE_SIZE_Y)},
        };


        public static Vector2 Velocity(Direction direction)
        {
            if (VELOCITY_MAP.TryGetValue(direction, out Vector2 ret))
            {
                return ret;
            }
            return Vector2.Zero;
        }


        public static Vector2I ToCoordinate(this Vector2 vector2)
        {
            return new((int)(vector2.X / TILE_SIZE_X), (int)(vector2.Y / TILE_SIZE_Y));
        }

        public static Vector2 ToPosition(this Vector2I vector2)
        {
            return new Vector2(vector2.X * TILE_SIZE_X, vector2.Y * TILE_SIZE_Y);
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