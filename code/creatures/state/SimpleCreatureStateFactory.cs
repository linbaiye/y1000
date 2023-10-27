using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures.state
{
    public class SimpleCreatureStateFactory : AbstractCreatureStateFactory
    {
        public override AbstractCreatureAttackState CreatureAttackState(AbstractCreature creature)
        {
            return new SimpleCreatureAttackState(creature, creature.Direction);
        }

        public override AbstractCreatureDieState CreatureDieState(AbstractCreature creature)
        {
            return new SimpleCreatureDieState(creature, creature.Direction);
        }

        public override AbstractCreatureHurtState CreatureHurtState(AbstractCreature creature)
        {
            return new SimpleCreatureHurtState(creature, creature.Direction);
        }

        public override AbstractCreatureIdleState CreatureIdleState(AbstractCreature creature)
        {
            return new SimpleCreatureIdleState(creature, creature.Direction);
        }

        public override AbstractCreatureMoveState CreatureMoveState(AbstractCreature creature, Direction newDirection)
        {
            return new SimpleCreatureMoveState(creature, newDirection);
        }
    }
}