using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.Source.Creature;

namespace y1000.code.creatures.state
{
    public class SimpleCreatureStateFactory : AbstractCreatureStateFactory
    {
        public override AbstractCreatureAttackState CreateAttackState(AbstractCreature creature)
        {
            return new SimpleCreatureAttackState(creature, creature.Direction);
        }

        public override AbstractCreatureDieState CreateDieState(AbstractCreature creature)
        {
            return new SimpleCreatureDieState(creature, creature.Direction);
        }

        public override AbstractCreatureHurtState CreateHurtState(AbstractCreature creature)
        {
            return new SimpleCreatureHurtState(creature, creature.Direction);
        }

        public override AbstractCreatureIdleState CreateIdleState(AbstractCreature creature)
        {
            return new SimpleCreatureIdleState(creature, creature.Direction);
        }

        public override AbstractCreatureMoveState CreateMoveState(AbstractCreature creature, Direction newDirection)
        {
            return new SimpleCreatureMoveState(creature, newDirection);
        }

        public static readonly SimpleCreatureStateFactory INSTANCE = new ();
    }
}