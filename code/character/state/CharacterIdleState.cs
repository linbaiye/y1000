using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.player;
using y1000.code.player.state;
using y1000.code.util;
using y1000.code.world;

namespace y1000.code.character.state
{
    public sealed class CharacterIdleState : PlayerIdleState, ICharacterState
    {
        public CharacterIdleState(Character player, Direction direction) : base(player, direction, CharacterStateFactory.INSTANCE)
        {
        }

        private void MoveOrTurn(Direction clickDirection)
        {
            var next = Creature.Coordinate.Next(clickDirection);
            if (((Character)Creature).CanMove(next))
            {
                Move(clickDirection);
            }
            else
            {
                Turn(clickDirection);
            }
        }

        public void OnMouseRightClick( Direction clickDirection)
        {
            MoveOrTurn(clickDirection);
        }

        public void OnMouseMotion(Direction direction)
        {
            MoveOrTurn(direction);
        }


        public void OnMouseLeftDoubleClick(bool ctrlPressed, bool shiftPressed, ICreature target)
        {
            if ((ctrlPressed && target is Player) || (shiftPressed && target is SimpleCreature))
            {
                StopAndChangeState(new CharacterEnfightState((Character)Creature, Direction, target));
            }
        }

    }
}