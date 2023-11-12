using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.entity
{
    public interface IEntity
    {
        Rect2I HoverRect();

        Point Coordinate { get; }

        long Id { get; }
    }
}