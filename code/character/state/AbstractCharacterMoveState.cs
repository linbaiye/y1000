using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.util;

namespace y1000.code.character.state
{
    public abstract class AbstractCharacterMoveState : AbstractCreatureMoveState, ICharacterState
    {

        private bool keepWalking;

        private Direction nextDirection;


        protected AbstractCharacterMoveState(Character character, Direction direction) : base(character, direction)
        {
            keepWalking = true;
            nextDirection = Direction;
        }

        public void OnMouseMotion( Direction direction)
        {
            nextDirection = direction;
        }
        
        public void OnMouseRightClick( Direction clickDirection)
        {
            keepWalking = true;
            nextDirection = clickDirection;
        }


        protected abstract AbstractCreatureState NextState();

        public override void OnAnimationFinised()
        {
            ChangeCoordinate();
            if (keepWalking)
            {
                var next = Creature.Coordinate.Next(nextDirection);
                if (((Character)Creature).CanMove(next))
                {
                    MoveTo(nextDirection, next);
                    PlayAnimation();
                    return;
                }
            }
            StopAndChangeState(NextState());
        }

        public void OnMouseRightReleased()
        {
            keepWalking = false;
        }
    }
}