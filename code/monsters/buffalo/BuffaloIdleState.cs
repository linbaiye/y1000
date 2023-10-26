using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.monsters.buffalo;
using y1000.code.player;

namespace y1000.code.monsters
{
    public sealed class BuffaloIdleState : AbstractCreatureIdleState
    {
        public BuffaloIdleState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
        }

        protected override ICreatureState CreateAttackState()
        {
            return new BuffaloAttackingState(Creature, Direction);
        }


        protected override ICreatureState CreateMoveState(Direction newDirection)
        {
            return new BuffaloMoveState(Creature, newDirection);
        }
    }
}