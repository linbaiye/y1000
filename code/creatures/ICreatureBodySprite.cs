using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.creatures
{
    public interface ICreatureBodySprite
    {
        Rect2I HoverRect();
    }
}