using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures.state;

namespace y1000.code.player.state
{
    public abstract class AbstractPlayerMoveState : AbstractCreatureMoveState
    {
        protected AbstractPlayerMoveState(Player player, Direction direction,
            Dictionary<Direction, int> spriteOffset, int spriteNumber, float step, AbstractCreatureStateFactory stateFactory, float speed) :
          base(player, direction, spriteOffset, spriteNumber, step, stateFactory, speed){}
    }
}