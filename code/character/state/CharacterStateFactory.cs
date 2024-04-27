using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog.Targets;
using y1000.code.creatures.state;
using y1000.code.player;
using y1000.Source.Creature;
using AbstractCreature = y1000.code.creatures.AbstractCreature;
using ICreature = y1000.code.creatures.ICreature;

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
            return new CharacterIdleState((OldCharacter)creature, creature.Direction);
        }

        public override AbstractCreatureMoveState CreateMoveState(AbstractCreature creature, Direction newDirection)
        {
            return new CharacterWalkState((OldCharacter)creature, newDirection);
        }

        public AbstractCharacterAttackState CreateAttackState(OldCharacter character, ICreature? target)
        {
            switch(character.EquippedWeapon)
            {
            }
            throw new NotSupportedException();
        }

        public static readonly CharacterStateFactory INSTANCE = new CharacterStateFactory();
    }
}