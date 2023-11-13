using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public partial class ChestSprite: Sprite2D
    {
        public override void _Process(double delta)
        {
            OffsetTexture positionedTexture = GetParent<Player>().ChestTexture;
            Offset = positionedTexture.Offset + new Vector2(18, -10);
            Texture = positionedTexture.Texture;
        }
    }
}