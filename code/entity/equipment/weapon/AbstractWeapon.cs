using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.entity.equipment.weapon
{
    public abstract class AbstractWeapon : IWeapon
    {
        private readonly long id;

        private readonly string spriteName;

        private readonly string name;

        protected AbstractWeapon(long id, string spriteName, string name)
        {
            this.id = id;
            this.spriteName = spriteName;
            this.name = name;
        }

        public abstract bool IsRange { get; }
        public string SpriteName => spriteName;
        public abstract string SpriteBasePath { get; }
        public string EntityName => name;
        public long Id => id;

        public abstract Vector2 Offset { get; }
    }
}