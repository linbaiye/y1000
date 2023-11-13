using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public abstract partial class AbstractBodyPart : Sprite2D
    {
        protected abstract OffsetTexture PositionedTexture {get;}

        public override void _Process(double delta)
        {
            OffsetTexture positionedTexture = PositionedTexture;
            Offset = positionedTexture.Offset;
            Texture = positionedTexture.Texture;
        }
    }
}