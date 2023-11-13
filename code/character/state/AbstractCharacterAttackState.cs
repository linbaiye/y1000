using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;

namespace y1000.code.character.state
{
    public abstract class AbstractCharacterAttackState : AbstractCreatureState
    {
        protected AbstractCharacterAttackState(AbstractCreature creature, Direction direction, AbstractCreatureStateFactory _stateFactory) : base(creature, direction, _stateFactory)
        {
        }
    }
}