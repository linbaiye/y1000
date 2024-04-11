using System;
using System.Collections.Generic;
using Godot;
using y1000.code;
using y1000.code.character.state;
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

        private IPrediction PredictRightClick(Character character, AbstractRightClickInput input)
        {
            if (character.CanMoveOneUnit(input.Direction))
            {
                return new MovePrediction(input, character.Coordinate, input.Direction);
            }
            else
            {
                return new TurnPrediction(input, character.Coordinate, input.Direction);
            }
        }

        private IClientEvent MoveByClick(Character character, AbstractRightClickInput input)
        {
            if (character.CanMoveOneUnit(input.Direction))
            {
                //character.ChangeState(CharacterMoveState.Create(character.IsMale, input));
            }
            else
            {
                character.Direction = input.Direction;
                character.ChangeState(Create(character.IsMale));
            }
            return new MovementEvent(input, character.Coordinate);
        }

        public void OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            HandleRightClick(character, rightClick);
        }

        private void HandleRightClick(Character character, AbstractRightClickInput rightClick)
        {
            var prediction = PredictRightClick(character, rightClick);
            var clientEvent = MoveByClick(character, rightClick);
            character.EmitMovementEvent(prediction, clientEvent);
        }


        public bool CanHandle(IInput input)
        {
            return input is AbstractRightClickInput;
        }

        public IPlayerState WrappedState => _idleState;

        public static CharacterIdleState ForMale()
        {
            var state = PlayerIdleState.StartFrom(true, 0);
            return new CharacterIdleState(state);
        }

        public static CharacterIdleState Create(bool forMale)
        {
            return ForMale();
        }

        public void OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease)
        {
        }

        public static CharacterIdleState Wrap(PlayerIdleState idleState)
        {
            return new CharacterIdleState(idleState);
        }

        public void OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion)
        {
            HandleRightClick(character, mousePressedMotion);
        }
    }
}