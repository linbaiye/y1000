using System;
using y1000.code.creatures;

namespace y1000.code.player
{
    public partial class AbstractPlayer : AbstractCreature, IPlayer
    {
        public void Bow()
        {
            throw new NotImplementedException();
        }

        public void Sit()
        {
            throw new NotImplementedException();
        }

        public state.IPlayerState PlayerState => (state.IPlayerState) CurrentState ;

        protected override SpriteContainer GetSpriteContainer()
        {
            return PlayerState.SpriteContainer;
        }
    }
}