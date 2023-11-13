using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;

namespace y1000.code.character.state
{
    public interface IStateFactory<C, S> where C : ICreature<S> where S : ICreatureState
    {
        S CreateIdleState(C creature, S currentState);
    }
}