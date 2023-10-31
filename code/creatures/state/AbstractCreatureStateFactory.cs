using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures.state
{
    public abstract class AbstractCreatureStateFactory
    {

        public abstract AbstractCreatureMoveState CreateMoveState(AbstractCreature creature, Direction newDirection);

        public abstract AbstractCreatureIdleState CreateIdleState(AbstractCreature creature);

        public abstract AbstractCreatureHurtState CreateHurtState(AbstractCreature creature);

        public abstract AbstractCreatureDieState CreateDieState(AbstractCreature creature);

        public abstract AbstractCreatureAttackState CreateAttackState(AbstractCreature creature);
        
    }
}