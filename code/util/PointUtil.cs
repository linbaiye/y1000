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


        private static Point Add(this Point point, int x, int y)
        {
            return new Point(point.X + x, point.Y + y);
        }

        public static Point Next(this Point current, Direction direction)
        {
            return direction switch
            {
                Direction.DOWN_LEFT => new Point(current.X - 1, current.Y + 1),
                Direction.LEFT => new Point(current.X - 1, current.Y),
                Direction.UP_LEFT => new Point(current.X - 1, current.Y - 1),
                Direction.UP => new Point(current.X, current.Y - 1),
                Direction.DOWN => new Point(current.X, current.Y + 1),
                Direction.UP_RIGHT => new Point(current.X + 1, current.Y - 1),
                Direction.RIGHT => new Point(current.X + 1, current.Y),
                Direction.DOWN_RIGHT => new Point(current.X + 1, current.Y + 1),
                _ => current
            };
        }

        public static bool IsNextTo(this Point point, Point another)
        {
            return Math.Abs(point.X - another.X) <= 1 && Math.Abs(point.Y - another.Y) <= 1;
        }


        public static Godot.Vector2 CoordinateToPixel(this Point point)
        {
            return new Godot.Vector2 (point.X * VectorUtil.TileSizeX, point.Y * VectorUtil.TileSizeY);
        }

        public static Direction? ComputeDirection(Point coordinate)
        {
            return null;
        }
    }
}