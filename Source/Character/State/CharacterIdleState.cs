using System;
using System.Collections.Generic;
using Godot;
using y1000.Source.Character.Event;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;
using y1000.Source.Player;

namespace y1000.Source.Character.State
{
    public class CharacterIdleState : ICharacterState
    {

        private readonly PlayerIdleState _idleState;

        private CharacterIdleState(PlayerIdleState state)
        {
            _idleState = state;
        }

        public void OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            HandleRightClick(character, rightClick);
        }

        private void HandleRightClick(Character character, AbstractRightClickInput rightClick)
        {
            if (!character.CanMoveOneUnit(rightClick.Direction))
            {
                if (character.Direction != rightClick.Direction)
                {
                    character.EmitEvent(new SetPositionPrediction(rightClick, character.Coordinate, rightClick.Direction), 
                        new MovementEvent(rightClick, character.Coordinate));
                    character.Direction = rightClick.Direction;
                    character.ChangeState(Create(character.IsMale));
                }
            }
            else
            {
                character.EmitEvent(new MovePrediction(rightClick, character.Coordinate, rightClick.Direction),
                    new MovementEvent(rightClick, character.Coordinate));
                var state = CharacterMoveState.Move(character.FootMagic, character.IsMale, rightClick);
                character.ChangeState(state);
            }
        }

        public bool CanHandle(IPredictableInput input)
        {
            return input is AbstractRightClickInput or AttackInput;
        }

        public void Attack(Character character, AttackInput input)
        {
            character.AttackKungFu?.Attack(character, input);
        }

        public IPlayerState WrappedState => _idleState;

        public static CharacterIdleState Create(bool male)
        {
            var state = PlayerIdleState.StartFrom(male, 0);
            return new CharacterIdleState(state);
        }

        public static CharacterIdleState Wrap(PlayerIdleState idleState)
        {
            return new CharacterIdleState(idleState);
        }

        public void OnWrappedPlayerAnimationFinished(Character character)
        {
            character.ChangeState(Create(character.IsMale));
        }

        public void OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion)
        {
            HandleRightClick(character, mousePressedMotion);
        }
    }
}