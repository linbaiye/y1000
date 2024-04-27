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
using y1000.Source.KungFu.Foot;
using y1000.Source.Player;
using y1000.Source.Util;

namespace y1000.Source.Character.State
{
    public class CharacterMoveState : ICharacterState
    {
        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

        private readonly AbstractRightClickInput _currentInput;

        private IRightClickInput? _lastInput;
        
        private readonly PlayerMoveState _playerMoveState;

        private CharacterMoveState(AbstractRightClickInput currentInput, PlayerMoveState playerMoveState)
        {
            _currentInput = currentInput;
            _playerMoveState = playerMoveState;
        }

        public bool CanHandle(IPredictableInput input)
        {
            return input is AbstractRightClickInput or MouseRightRelease;
        }

        private void ContinueMove(Character character, AbstractRightClickInput input)
        {
            var next = character.Coordinate.Move(input.Direction);
            if (!character.WrappedPlayer().Map.Movable(next))
            {
                character.EmitEvent(
                    SetPositionPrediction.Overflow(input, character.Coordinate, character.Direction),
                    new MovementEvent(input, character.Coordinate));
                character.ChangeState(CharacterIdleState.Create(character.IsMale));
            }
            else
            {
                character.EmitEvent(
                    new MovePrediction(input, character.Coordinate, input.Direction),
                    new MovementEvent(input, character.Coordinate));
                character.ChangeState(Move(character.FootMagic, character.IsMale, input));
            }
        }

        public void OnWrappedPlayerAnimationFinished(Character character)
        {
            if (_lastInput is MouseRightRelease)
            {
                character.EmitEvent(
                    SetPositionPrediction.Overflow(_lastInput, character.Coordinate, character.Direction),
                    new MovementEvent(_lastInput, character.Coordinate));
                character.ChangeState(CharacterIdleState.Create(character.IsMale));
            }
            else if (_lastInput is AbstractRightClickInput input)
            {
                ContinueMove(character, input);
            }
            else if (_lastInput == null)
            {
                var nextInput = InputFactory.CreateRightMousePressedMotion(_currentInput.Direction);
                ContinueMove(character, nextInput);
            }
        }

        public IPlayerState WrappedState => _playerMoveState;
        
        
        public void OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            _lastInput = rightClick;
        }

        public void OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease)
        {
            _lastInput = mouseRightRelease;
        }

        public void OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion)
        {
            _lastInput = mousePressedMotion;
        }

        public static CharacterMoveState Move(IFootKungFu? magic, bool male, AbstractRightClickInput rightClickInput)
        {
            if (magic != null)
            {
                return magic.CanFly ? Fly(male, rightClickInput) : Run(male, rightClickInput);
            }
            return Walk(male, rightClickInput);
        }


        private static CharacterMoveState Walk(bool male, AbstractRightClickInput input)
        {
            return new CharacterMoveState(input, PlayerMoveState.WalkTowards(male, input.Direction));
        }

        private static CharacterMoveState Run(bool male, AbstractRightClickInput input)
        {
            return new CharacterMoveState(input, PlayerMoveState.RunTowards(male, input.Direction));
        }

        private static CharacterMoveState Fly(bool male, AbstractRightClickInput input)
        {
            return new CharacterMoveState(input, PlayerMoveState.FlyTowards(male, input.Direction));
        }
    }
}