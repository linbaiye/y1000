using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.entity.equipment
{
    public abstract class AbstractArmor : IArmor
    {

        private readonly long id;

        private readonly string spriteName;

        private readonly string armorName;

        private readonly bool male;

        protected AbstractArmor(long id, string spriteName, string armorName, bool male)
        {
            this.id = id;
            this.spriteName = spriteName;
            this.armorName = armorName;
            this.male = male;
        }

        public bool IsMale => male;

        public string SpriteName => spriteName;

        public string EntityName => armorName;

        public long Id => id;
        public Vector2I Coordinate { get; }

        public abstract string SpriteBasePath { get; }
        public abstract Vector2 Offset { get; }
    }
}