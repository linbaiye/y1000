using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.player.state;

namespace y1000.code.player
{
    public partial class Player : AbstractCreature, IPlayer
    {
        public override long Id => throw new NotImplementedException();

        public void Bow()
        {
            throw new NotImplementedException();
        }

        public void Sit()
        {
            throw new NotImplementedException();
        }

        protected override SpriteContainer GetSpriteContainer()
        {
            return ((state.IPlayerState)CurrentState).SpriteContainer;
        }

        public bool IsMale()
        {
            return true;
        }

    }
}