using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public partial class HatSprite : Sprite2D
    {

        private IPlayer? player;

        public override void _Ready()
        {
            player = GetParent<Player>();
        }


        public override void _Process(double delta)
        {
            if (player != null)
            {
                OffsetTexture? positionedTexture = player.HatTexture;
                if (positionedTexture != null)
                {
                    Offset = positionedTexture.Offset + new Vector2(18, -9);
                    Texture = positionedTexture.Texture;
                }
            }
        }

    }
}