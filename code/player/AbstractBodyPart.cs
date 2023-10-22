using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public abstract partial class AbstractBodyPart : Sprite2D
    {
        protected abstract PositionedTexture PositionedTexture {get;}

        public override void _Ready()
        {
        }

        public override void _Process(double delta)
        {
            PositionedTexture positionedTexture = PositionedTexture;
            Position = positionedTexture.Position;
            Texture = positionedTexture.Texture;
        }
    }
}