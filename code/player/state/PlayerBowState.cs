using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;

namespace y1000.code.player.state
{
    public class PlayerBowState : AbstractCreatureState
    {
        public PlayerBowState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
        }

        public override State State => State.BOW;

        protected override int SpriteOffset => throw new NotImplementedException();

        protected override SpriteContainer SpriteContainer => throw new NotImplementedException();
    }
}