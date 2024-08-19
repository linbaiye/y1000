using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.world
{
    public class EmptyRealm : IRealm
    {
        public bool CanMove(Vector2I coordinate)
        {
            return false;
        }
    }
}