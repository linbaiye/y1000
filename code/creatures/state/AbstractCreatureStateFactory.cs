using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures.state
{
    public abstract class AbstractCreatureStateFactory
    {

        public abstract AbstractCreatureMoveState CreatureMoveState(AbstractCreature creature, Direction newDirection);

        public abstract AbstractCreatureIdleState CreatureIdleState(AbstractCreature creature);

        public abstract AbstractCreatureHurtState CreatureHurtState(AbstractCreature creature);

        public abstract AbstractCreatureDieState CreatureDieState(AbstractCreature creature);

        public abstract AbstractCreatureAttackState CreatureAttackState(AbstractCreature creature);
        
    }
}