using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.world
{
    public interface IRealm
    {
        bool CanMove(Vector2I coordinate);

        public static readonly IRealm Empty = new EmptyRealm();
    }
}