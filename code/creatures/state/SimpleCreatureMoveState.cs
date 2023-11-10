using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures.state
{
    public class SimpleCreatureMoveState : AbstractCreatureMoveState
    {
        public SimpleCreatureMoveState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {

        }
    }
}