using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;

namespace y1000.code.monsters.buffalo
{
    public class BuffaloAttackingState : AbstractCreatureAttackState
    {
        public BuffaloAttackingState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
        }

        protected override ICreatureState CreateIdleState()
        {
            return new BuffaloIdleState(Creature, Direction);
        }
    }
}