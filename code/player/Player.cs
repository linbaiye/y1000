using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.entity.equipment.chest;
using y1000.code.player.state;

namespace y1000.code.player
{
    public partial class Player : AbstractCreature, IPlayer
    {
        private IChestArmor? chestArmor; 

        public override long Id => throw new NotImplementedException();

        public OffsetTexture? ChestTexture
        {
            get
            {
                return chestArmor != null ? ((IPlayerState)CurrentState).ChestTexture(SpriteNumber, chestArmor) : null;
            }
        }

        public IChestArmor? ChestArmor
        {
            get { return chestArmor; }
            set
            {
                if (value != null && IsMale() == value.IsMale)
                {
                    chestArmor = value;
                }
            }
        }

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

        void IPlayer.Sit()
        {
            throw new NotImplementedException();
        }

        void IPlayer.Bow()
        {
            throw new NotImplementedException();
        }

        bool IPlayer.IsMale()
        {
            throw new NotImplementedException();
        }
    }
}