using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.character.state.input;
using y1000.code.character.state.Prediction;
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
        public CharacterIdleState(OldCharacter player, Direction direction) : base(player, direction, CharacterStateFactory.INSTANCE)
        {
        }

        public void OnMouseRightClick(Direction clickDirection)
        {
            ((OldCharacter)Creature).MoveOrTurn(clickDirection);
            
        }



        public void OnMouseMotion(Direction direction)
        {
            ((OldCharacter)Creature).MoveOrTurn(direction);
        }

        public void OnMouseRightReleased(OldCharacter character, MouseRightRelease mouseRightRelease)
        {
            character.SendActAndSavePredict(mouseRightRelease, null);
        }

        public void OnMouseRightClicked(OldCharacter character, MouseRightClick rightClick)
        {
            character.SendActAndSavePredict(rightClick, () => {
                var next = character.Coordinate.Next(rightClick.Direction);
                if (character.CanMove(next))
                {
                    StopAndChangeState(new CharacterWalkState(character, rightClick));
                }
                else
                {
                    SetDirection(rightClick.Direction);
                }
            });
        }

        public void OnMouseLeftDoubleClick(bool ctrlPressed, bool shiftPressed, ICreature target)
        {
            if ((ctrlPressed && target is Player) || (shiftPressed && target is SimpleCreature))
            {
                StopAndChangeState(new CharacterEnfightState((OldCharacter)Creature, Direction, target));
            }
        }

        public IPrediction Predict(OldCharacter character)
        {
            throw new NotImplementedException();
        }
    }
}