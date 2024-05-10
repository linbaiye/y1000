using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;
using y1000.code.player.skill;
using y1000.code.player.state;
using y1000.Source.Animation;

namespace y1000.code.player
{
    public partial class Player : AbstractCreature, IPlayer
    {
        private ChestArmor? chestArmor; 

        private Hat? hat; 

        private Trousers? trousers; 

        private IWeapon? weapon;

        public override long Id => throw new NotImplementedException();

        private IBufa? bufa = null;

        public IBufa? Bufa => bufa;


        public OffsetTexture? ChestTexture
        {
            get
            {
                return chestArmor != null ? ((IPlayerState)CurrentState).ChestTexture(SpriteNumber, chestArmor) : null;
            }
        }

        public IWeapon? Weapon {
            get { return weapon; }
            set
            {
                weapon = value;
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

        private IPlayerState MyState => (IPlayerState)CurrentState;

        public OffsetTexture? HatTexture => hat != null ? ((IPlayerState)CurrentState).HatTexture(SpriteNumber, hat) : null;

        public Hat? Hat
        {
            get { return hat; }
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
            get { return trousers; }
            set
            {
                if (value != null && IsMale() == value.IsMale)
                {
                    trousers = value;
                }
            }
        }

        public OffsetTexture? TrousersTexture => trousers != null ? ((IPlayerState)CurrentState).TrousersTexture(SpriteNumber, trousers) : null;

        public OffsetTexture? WeaponTexture => Weapon != null ? MyState.WeaponTexture(SpriteNumber, Weapon) : null;


        public void Bow()
        {
            throw new NotImplementedException();
        }

        public void Sit()
        {
            MyState.Sit();
        }

        public bool IsMale()
        {
            return true;
        }

        public void EnableBufa(IBufa bufa)
        {
            GetNode<Body>("Body").OnBufaEnabled();
            this.bufa = bufa;
        }


        public void DisableBufa(IBufa bufa)
        {
            GetNode<Body>("Body").OnBufaDisabled();
            this.bufa = null;
        }

        public void PressBufa(IBufa bufa)
        {
            if (this.bufa != null)
            {
                this.bufa = null;
            }
            else if (MyState.PressBufa(bufa))
            {
                this.bufa = bufa;
            }
        }
    }
}