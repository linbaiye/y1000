using System;
using y1000.code.creatures;
using y1000.code.entity.equipment.chest;

namespace y1000.code.player
{
    public abstract partial class AbstractPlayer : AbstractCreature, IPlayer
    {
        public abstract OffsetTexture? ChestTexture { get; }
        public abstract IChestArmor ChestArmor { get; set; }

        public void Bow()
        {
            throw new NotImplementedException();
        }

        public void Sit()
        {
            throw new NotImplementedException();
        }

        public bool IsMale()
        {
            return true;
        }

    }
}