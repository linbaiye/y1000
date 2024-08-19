using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.Source.Animation;

namespace y1000.code.player
{
    public abstract partial class AbstractBodyPart : Sprite2D
    {
        protected abstract OffsetTexture OffsetTexture {get;}

        public override void _Process(double delta)
        {
            OffsetTexture positionedTexture = OffsetTexture;
            Offset = positionedTexture.Offset;
            Texture = positionedTexture.Texture;
        }
    }
}