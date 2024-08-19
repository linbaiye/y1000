using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.entity.equipment.weapon
{
    public class Sword : AbstractWeapon
    {
        private static readonly Vector2 OFFSET = new Vector2(18, -9);
        public Sword(long id, string spriteName, string name) : base(id, spriteName, name)
        {
        }

        public override bool IsRange => false;


        public override string SpriteBasePath => "weapon/sword/" + SpriteName;

        public override Vector2 Offset => OFFSET;
    }
}