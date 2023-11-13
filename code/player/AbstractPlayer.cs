using System;
using y1000.code.creatures;

namespace y1000.code.player
{
    public abstract partial class AbstractPlayer : AbstractCreature, IPlayer
    {

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