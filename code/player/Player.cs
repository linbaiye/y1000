using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.player.state;

namespace y1000.code.player
{
    public partial class Player : AbstractCreature, IPlayer
    {
        private ChestArmor? chestArmor; 

        private Hat? hat; 

        private Trousers? trousers; 

        public override long Id => throw new NotImplementedException();

        public OffsetTexture? ChestTexture
        {
            get
            {
                return chestArmor != null ? ((IPlayerState)CurrentState).ChestTexture(SpriteNumber, chestArmor) : null;
            }
        }

        public ChestArmor? ChestArmor
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

        public OffsetTexture? HatTexture => hat != null ? ((IPlayerState)CurrentState).HatTexture(SpriteNumber, hat) : null;

        public Hat? Hat
        {
            get {return hat;}
            set 
            {
                if (value != null && IsMale() == value.IsMale)
                {
                    hat = value;
                }
            }
        }

        public Trousers? Trousers 
        {
            get {return trousers;}
            set 
            {
                if (value != null && IsMale() == value.IsMale)
                {
                    trousers = value;
                }
            }
        }

        public OffsetTexture? TrousersTexture => trousers != null ? ((IPlayerState)CurrentState).TrousersTexture(SpriteNumber, trousers) : null;

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