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
                character.ChangeState(CharacterIdleState.Create());
            }
            else
            {
                character.EmitEvent(
                    new MovePrediction(input, character.Coordinate, input.Direction),
                    new MovementEvent(input, character.Coordinate));
                character.ChangeState(Move(character.FootMagic, input));
            }
        }

        public void OnWrappedPlayerAnimationFinished(Character character)
        {
            if (_lastInput is MouseRightRelease)
            {
                character.EmitEvent(
                    SetPositionPrediction.Overflow(_lastInput, character.Coordinate, character.Direction),
                    new MovementEvent(_lastInput, character.Coordinate));
                character.ChangeState(CharacterIdleState.Create());
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

        public static CharacterMoveState Move(IFootKungFu? magic, AbstractRightClickInput rightClickInput)
        {
            if (magic != null)
            {
                return magic.CanFly ? Fly(rightClickInput) : Run(rightClickInput);
            }
            return Walk(rightClickInput);
        }


        private static CharacterMoveState Walk(AbstractRightClickInput input)
        {
            return new CharacterMoveState(input, PlayerMoveState.WalkTowards(input.Direction));
        }

        private static CharacterMoveState Run(AbstractRightClickInput input)
        {
            return new CharacterMoveState(input, PlayerMoveState.RunTowards(input.Direction));
        }

        private static CharacterMoveState Fly(AbstractRightClickInput input)
        {
            return new CharacterMoveState(input, PlayerMoveState.FlyTowards(input.Direction));
        }
    }
}