using System;
using System.Collections.Generic;
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

        public static Direction MouseDirection(this Vector2 mousePosition)
        {
            var angle = Mathf.Snapped(mousePosition.Angle(), Mathf.Pi / 3) / (Mathf.Pi / 4);
            int dir = Mathf.Wrap((int)angle, -1, 8);
            return dir switch
            {
                -1 => Direction.RIGHT,
                0 => Direction.DOWN_RIGHT,
                1 => Direction.DOWN,
                2 => Direction.DOWN_LEFT,
                3 => Direction.LEFT,
                4 => Direction.UP_LEFT,
                5 => Direction.UP,
                6 => Direction.UP_RIGHT,
                _ => throw new NotSupportedException(),
            };
        }
    }
}