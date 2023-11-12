using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.util
{
    public static class PointUtil
    {

        public static ISet<Point> Neibours(this Point point)
        {
            return new HashSet<Point>
            {
                point.Add(0, -1),
                point.Add(1, -1),
                point.Add(1, 0),
                point.Add(1, 1),
                point.Add(0, 1),
                point.Add(-1, 1),
                point.Add(-1, 0),
                point.Add(-1, -1)
            };
        }


        public static Vector2I ToVector2I(this Godot.Vector2 vector2)
        {
            return new((int)vector2.X, (int)vector2.Y);
        }


        public static bool IsSame(this Point point, Vector2I another)
        {
            return point.X.Equals(another.X) && point.Y.Equals(another.Y);
        }


        public static Point Add(this Point point, int x, int y)
        {
            return new Point(point.X + x, point.Y + y);
        }

        public static Point Next(this Point current, Direction direction)
        {
            if (direction == Direction.DOWN_LEFT)
                return new Point(current.X - 1, current.Y + 1);
            else if (direction == Direction.LEFT)
                return new Point(current.X - 1, current.Y);
            else if (direction == Direction.UP_LEFT)
                return new Point(current.X - 1, current.Y - 1);
            else if (direction == Direction.UP)
                return new Point(current.X, current.Y - 1);
            else if (direction == Direction.DOWN)
                return new Point(current.X, current.Y + 1);
            else if (direction == Direction.UP_RIGHT)
                return new Point(current.X + 1, current.Y - 1);
            else if (direction == Direction.RIGHT)
                return new Point(current.X + 1, current.Y);
            else if (direction == Direction.DOWN_RIGHT)
                return new Point(current.X + 1, current.Y + 1);
            return current;
        }

        public static Godot.Vector2 CoordinateToPixel(this Point point)
        {
            return new Godot.Vector2 (point.X * VectorUtil.TILE_SIZE_X, point.Y * VectorUtil.TILE_SIZE_Y);
        }
    }
}