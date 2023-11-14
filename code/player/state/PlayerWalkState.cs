using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures.state;

namespace y1000.code.player.state
{
    public class PlayerWalkState: AbstractPlayerWalkState
    {
        public PlayerWalkState(Player _player, Direction direction) : base(_player, direction, PlayerStateFactory.INSTANCE)
        {
        }

        public PlayerWalkState(Player _player, Direction direction, AbstractCreatureStateFactory stateFactory) : base(_player, direction, stateFactory)
        {
        }

        public override State State => State.WALK;

        public override void OnAnimationFinised()
        {
            StopAndChangeState(StateFactory.CreateIdleState(Creature));
        }


    }
}