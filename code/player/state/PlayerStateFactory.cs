using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures.state;
using y1000.Source.Creature;
using AbstractCreature = y1000.code.creatures.AbstractCreature;

namespace y1000.code.player.state
{
    public class PlayerStateFactory : AbstractCreatureStateFactory
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
            return new PlayerIdleState((Player) creature, creature.Direction);
        }

        public override AbstractCreatureMoveState CreateMoveState(AbstractCreature creature, Direction newDirection)
        {
            return new PlayerWalkState((Player) creature, creature.Direction);
        }

        public static readonly PlayerStateFactory INSTANCE = new PlayerStateFactory();
    }
}