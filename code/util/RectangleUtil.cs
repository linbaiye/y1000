using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.util
{
    public static class RectangleUtil
    {
        public static bool HasVector(this Rectangle rectangle, Vector2 vector2)
        {
            return rectangle.Contains(new Point((int)vector2.X, (int)vector2.Y));
        }
    }
}