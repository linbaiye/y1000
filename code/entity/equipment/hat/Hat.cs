using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.entity.equipment.hat
{
    public sealed class Hat : AbstractArmor
    {
        public Hat(long id, string spriteName, string armorName, bool male) : base(id, spriteName, armorName, male)
        {
        }

        public override string SpriteBasePath => "armor/" + (IsMale ? "male/": "female/") + "hat/" + SpriteName;

        public override Vector2 Offset => throw new NotImplementedException();
    }
}