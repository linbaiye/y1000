using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.entity.equipment.trousers
{
    public class Trousers : AbstractArmor
    {
        public Trousers(long id, string spriteName, string armorName, bool male) : base(id, spriteName, armorName, male)
        {
        }

        public override string SpriteBasePath => "armor/" + (IsMale ? "male/": "female/") + "trouser/" + SpriteName;
    }
}