using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;

namespace y1000.code.character.state
{
    public class CharacterStateFactory : AbstractCreatureStateFactory
    {
        public override AbstractCreatureAttackState CreateAttackState(AbstractCreature creature)
        {
            throw new NotImplementedException();
        }

        public override AbstractCreatureDieState CreateDieState(AbstractCreature creature)
        {
            throw new NotImplementedException();
        }

        public override AbstractCreatureHurtState CreateHurtState(AbstractCreature creature)
        {
            throw new NotImplementedException();
        }

        public override AbstractCreatureIdleState CreateIdleState(AbstractCreature creature)
        {
            return new CharacterIdleState((Character)creature, creature.Direction);
        }

        public override AbstractCreatureMoveState CreateMoveState(AbstractCreature creature, Direction newDirection)
        {
            return new CharacterWalkState((Character)creature, newDirection);
        }

        public static readonly CharacterStateFactory INSTANCE = new CharacterStateFactory();
    }
}