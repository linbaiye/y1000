using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.entity.equipment.chest
{
    public sealed class ChestArmor : IChestArmor
    {
        private readonly bool male;
        private readonly string name;
        private readonly string spriteName;

        public ChestArmor(bool male, string name, string spriteName)
        {
            this.male = male;
            this.name = name;
            this.spriteName = spriteName;
        }



        public bool IsMale => male;

        public string EntityName => name;

        public string SpriteName => spriteName;

        public long Id => throw new NotImplementedException();

    }
}