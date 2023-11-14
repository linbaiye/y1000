using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;

namespace y1000.code.player.state
{
    public abstract class AbstractPlayerAttackState : AbstractCreatureState, IPlayerState
    {
        public override State State => State.ATTACKING;

        private static readonly Random RANDOM = new Random();

        protected readonly int actionType;

        protected const int BELOW_50 = 0;

        protected const int ABOVE_50 = 1;


        protected AbstractPlayerAttackState(AbstractPlayer creature, Direction direction,
        AbstractCreatureStateFactory _stateFactory) : base(creature, direction, _stateFactory)
        {
            actionType = RANDOM.Next(BELOW_50, ABOVE_50 + 1);
        }

        public abstract OffsetTexture ChestTexture(int animationSpriteNumber, IChestArmor armor);
    }
}