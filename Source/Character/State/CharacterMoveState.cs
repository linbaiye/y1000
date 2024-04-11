using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using NLog;
using y1000.code;
using y1000.Source.Character.Event;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;
using y1000.Source.Player;

namespace y1000.Source.Character.State
{
    public class CharacterMoveState : ICharacterState
    {

        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

        private readonly AbstractRightClickInput _currentInput;

        private IInput? _lastInput;
        
        private readonly PlayerWalkState _playerWalkState;
        
        public CharacterMoveState(AbstractRightClickInput currentInput, PlayerWalkState playerWalkState)
        {
            _currentInput = currentInput;
            _playerWalkState = playerWalkState;
        }

        public bool CanHandle(IInput input)
        {
            return input is AbstractRightClickInput or MouseRightRelease;
        }

        public IPlayerState WrappedState => _playerWalkState;

        public void OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            _lastInput = rightClick;
        }

        public void Process(Character character, long deltaMillis)
        {
            if (_lastInput is MouseRightRelease)
            {
                character.ChangeState(CharacterIdleState.Create(character.IsMale));
                character.EmitMovementEvent(
                    SetPositionPrediction.Overflow(_lastInput, character.Coordinate, character.Direction),
                    new MovementEvent(_lastInput, character.Coordinate));
            }
            else if (_lastInput is AbstractRightClickInput input)
            {
                //character.ChangeState(Create(character.IsMale, input));
                character.EmitMovementEvent(
                    new MovePrediction(input, character.Coordinate, character.Direction),
                    new MovementEvent(input, character.Coordinate));
            }
            else if (_lastInput == null)
            {
                var nextInput = InputFactory.CreateRightMousePressedMotion(_currentInput.Direction);
                //character.ChangeState(Create(character.IsMale, nextInput));
                character.EmitMovementEvent(
                    new MovePrediction(nextInput, character.Coordinate, character.Direction),
                    new MovementEvent(nextInput, character.Coordinate));
            }
        }

        public void OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease)
        {
            _lastInput = mouseRightRelease;
        }

        public void OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion)
        {
            _lastInput = mousePressedMotion;
        }
    }
}