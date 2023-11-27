using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.entity.equipment.chest
{
    public sealed class ChestArmor : AbstractArmor
    {
        public ChestArmor(bool male, string name, string spriteName) : base(0, spriteName, name, male)
        {
        }

        public override string SpriteBasePath => "armor/" + (IsMale ? "male/": "female/") + "chest/" + SpriteName;

        public override Vector2 Offset => throw new NotImplementedException();

    }
}