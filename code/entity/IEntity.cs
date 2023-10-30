using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.entity
{
    public interface IEntity
    {
        Rectangle Rectangle() {return System.Drawing.Rectangle.FromLTRB(0, 0, 0, 0); }

        long Id() { return 0;}
    }
}