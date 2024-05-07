using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.Source.Entity.Animation;

namespace y1000.code.player
{
    public partial class HatSprite : AbstractEquipmentSprite
    {

        private static readonly Vector2 OFFSET = new Vector2(18, -9);

        protected override OffsetTexture? GetOffsetTexture(IPlayer p)
        {
            var hat = p.HatTexture;
            return hat?.Add(OFFSET);
        }
    }
}