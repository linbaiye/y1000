using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
using y1000.Source.Animation;

namespace y1000.code.player
{
    public partial class PlayerBodySprite: AbstractCreatureBodySprite
    {

        protected override OffsetTexture GetPositionedTexture()
        {
            var parent = GetParent<AbstractCreature>();
            var texture = parent.BodyTexture;
            return new OffsetTexture(texture.Texture, texture.Offset + new Vector2(16, -12));
        }
    }
}