using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public interface IInterpolation
    {

        public long Id {get;}

        bool DurationEnough(long elapsed);

        OffsetTexture BodyTexture(OtherPlayer player, long elapsed);

        Vector2 Position { get; }
    }
}