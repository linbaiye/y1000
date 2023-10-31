using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.entity
{
    public interface IEntity
    {
        Rectangle CollisionRect();

        Point Coordinate { get; }

        long Id() { return 0;}
    }
}