using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;

namespace y1000.code.player
{
    public partial class PlayerBodySprite: AbstractCreatureBodySprite
    {
        protected override PositionedTexture GetPositionedTexture()
        {
            var parent = GetParent<AbstractCreature>();
            var texture = parent.BodyTexture;
            return new PositionedTexture(texture.Texture, texture.Offset + new Vector2(16, -12));
        }


        protected override int GetTextureWidth()
        {
            return Texture.GetWidth() - 20;
        }
    }
}