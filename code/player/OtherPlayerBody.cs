using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.util;

namespace y1000.code.player
{
    public partial class OtherPlayerBody: Sprite2D
    {

        protected OffsetTexture? OffsetTexture => GetParent<OtherPlayer>().BodyTexutre;

        //protected abstract OffsetTexture OffsetTexture {get;}
        public override void _Process(double delta)
        {
            OffsetTexture? positionedTexture = OffsetTexture;
            if (positionedTexture != null)
            {
                Offset = positionedTexture.Offset;
                Texture = positionedTexture.Texture;
            }
        }
    }
}