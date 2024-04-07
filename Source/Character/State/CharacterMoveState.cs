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

namespace y1000.Source.Character.State
{
    public class CharacterMoveState : AbstractCharacterState
    {
        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new Dictionary<Direction, int>()
        {
            { Direction.UP, 0},
            { Direction.UP_RIGHT, 6},
            { Direction.RIGHT, 12},
            { Direction.DOWN_RIGHT, 18},
            { Direction.DOWN, 24},
            { Direction.DOWN_LEFT, 30},
            { Direction.LEFT, 36},
            { Direction.UP_LEFT, 42},
        };

        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

        private const int SpriteLengthMillis = 100;

        private readonly AbstractRightClickInput _currentInput;

        private IInput? _lastInput;

        public CharacterMoveState(AnimatedSpriteManager spriteManager, AbstractRightClickInput currentInput) : base(spriteManager)
        {
            _currentInput = currentInput;
        }

        public override bool CanHandle(IInput input)
        {
            return input is AbstractRightClickInput or MouseRightRelease;
        }

        public override void OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            _lastInput = rightClick;
        }

        private IPrediction PredictNextMove(Character character, AbstractRightClickInput input)
        {
            // The point when this move completes.
            var dest = character.Coordinate.Move(character.Direction);
            var newCoor = dest.Move(input.Direction);
            return character.CanMoveTo(newCoor) ? MovePrediction.Overflow(input, newCoor, input.Direction)
                : SetPositionPrediction.Overflow(input, dest, input.Direction);
        }

        public IPrediction Predict(Character character, MouseRightClick rightClick)
        {
            return PredictNextMove(character, rightClick);
        }

        public override void Process(Character character, long deltaMillis)
        {
            if (ElpasedMillis == 0)
            {
                character.Direction = _currentInput.Direction;
            }
            ElpasedMillis += deltaMillis;
            int animationLengthMillis = SpriteManager.AnimationLength;
            var velocity = VectorUtil.Velocity(character.Direction);
            character.Position += velocity * ((float)deltaMillis / animationLengthMillis);
            if (ElpasedMillis <= animationLengthMillis)
            {
                return;
            }
            character.Position = character.Position.Snapped(VectorUtil.TILE_SIZE);
            LOGGER.Debug("Moved to coordinate {0}.", character.Coordinate);
            if (_lastInput is MouseRightRelease)
            {
                character.ChangeState(CharacterIdleState.Create(character.IsMale));
                character.EmitMovementEvent(
                    SetPositionPrediction.Overflow(_lastInput, character.Coordinate, character.Direction),
                    new MovementEvent(_lastInput, character.Coordinate));
            }
            else if (_lastInput is AbstractRightClickInput input)
            {
                character.ChangeState(Create(character.IsMale, input));
                character.EmitMovementEvent(
                    new MovePrediction(input, character.Coordinate, character.Direction),
                    new MovementEvent(input, character.Coordinate));
            }
            else if (_lastInput == null)
            {
                var nextInput = InputFactory.CreateRightMousePressedMotion(_currentInput.Direction);
                character.ChangeState(Create(character.IsMale, nextInput));
                character.EmitMovementEvent(
                    new MovePrediction(nextInput, character.Coordinate, character.Direction),
                    new MovementEvent(nextInput, character.Coordinate));
            }
        }

        public static CharacterMoveState Create(bool forMale, AbstractRightClickInput input)
        {
            var asm = AnimatedSpriteManager.Normal(SpriteLengthMillis, SPRITE_OFFSET, SpriteContainer.LoadMalePlayerSprites("N02"));
            return new CharacterMoveState(asm, input);
        }

        public override void OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease)
        {
            _lastInput = mouseRightRelease;
        }

        public override void OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion)
        {
            _lastInput = mousePressedMotion;
        }
    }
}