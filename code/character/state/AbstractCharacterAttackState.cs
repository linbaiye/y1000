using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures.state;
using y1000.Source.Creature;
using AbstractCreature = y1000.code.creatures.AbstractCreature;

namespace y1000.code.character.state
{
    public abstract class AbstractCharacterAttackState : AbstractCreatureState
    {
        protected AbstractCharacterAttackState(AbstractCreature creature, Direction direction, AbstractCreatureStateFactory _stateFactory) : base(creature, direction, _stateFactory)
        {
        }
    }
}