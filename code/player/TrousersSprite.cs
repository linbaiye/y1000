using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.Source.Entity.Animation;

namespace y1000.code.player
{
    public partial class TrousersSprite : AbstractEquipmentSprite
    {
        private static readonly Vector2 OFF = new Vector2(18, -10);
        protected override OffsetTexture? GetOffsetTexture(IPlayer p)
        {
            return p.TrousersTexture?.Add(OFF);
        }
    }
}