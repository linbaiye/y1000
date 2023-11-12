using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
using y1000.code.player;
using y1000.code.player.state;
using y1000.code.util;
using y1000.code.world;

namespace y1000.code.character.state
{
    public class CharacterWalkState : AbstractPlayerWalkState, ICharacterState
    {

        private bool keepWalking;
        private Direction nextDirection;

        public override State State => State.WALK;

        public CharacterWalkState(Player _player, Direction direction) : base(_player, direction, CharacterStateFactory.INSTANCE)
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
            StopAndChangeState(StateFactory.CreateIdleState(Creature));
        }

        public void OnMouseRightReleased()
        {
            keepWalking = false;
        }
    }
}