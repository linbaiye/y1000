using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.Source.Animation;

namespace y1000.code.player
{
    public abstract partial class AbstractEquipmentSprite : Sprite2D
    {
        private IPlayer? player;

        public override void _Ready()
        {
            player = GetParent<Player>();
        }

        protected abstract OffsetTexture? GetOffsetTexture(IPlayer p);

        public override void _Process(double delta)
        {
            if (player != null)
            {
                OffsetTexture? positionedTexture = GetOffsetTexture(player);
                if (positionedTexture != null)
                {
                    Offset = positionedTexture.Offset;
                    Texture = positionedTexture.Texture;
                }
            }
        }
    }
}