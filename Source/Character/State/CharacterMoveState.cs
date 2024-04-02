using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code;
using y1000.code.character.state;
using y1000.code.character.state.input;
using y1000.code.character.state.Prediction;
using y1000.code.player;
using y1000.code.util;

namespace y1000.Source.Character.State
{
    public class CharacterMoveState : AbstractCharacterState
    {
        public static readonly Dictionary<Direction, int> SPRITE_OFFSET = new Dictionary<Direction, int>()
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

        private const int SPRITE_LENGTH_MILLIS = 100;

        private readonly AbstractRightClickInput _currentInput;

        private IInput? _lastInput;

        public CharacterMoveState(AnimatedSpriteManager spriteManager, AbstractRightClickInput currentInput) : base(spriteManager)
        {
            _currentInput = currentInput;
        }

        public override bool CanHandle(IInput input)
        {
            return _lastInput == null && (input is RightMousePressedMotion || input is MouseRightRelease || input is MouseRightClick);
        }

        public override void OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            _lastInput ??= rightClick;
        }

        private IPrediction NextMovePredict(Character character, AbstractRightClickInput input)
        {
            var newCoor = character.Coordinate.Move(character.Direction);
            return new MovePrediction(input, newCoor, input.Direction);;
        }

        public override IPrediction Predict(Character character, MouseRightClick rightClick)
        {
            return NextMovePredict(character, rightClick);
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
            if (_lastInput is MouseRightRelease)
            {
                character.ChangeState(CharacterIdleState.Create(character.IsMale));
            }
            else if (_lastInput is AbstractRightClickInput input)
            {
                character.ChangeState(Create(character.IsMale, input));
            }
            else
            {
                character.ChangeState(Create(character.IsMale, _currentInput));
            }
        }

        public static CharacterMoveState Create(bool forMale, AbstractRightClickInput input)
        {
            var asm = AnimatedSpriteManager.Normal(SPRITE_LENGTH_MILLIS, SPRITE_OFFSET, SpriteContainer.LoadMalePlayerSprites("N02"));
            return new CharacterMoveState(asm, input);
        }

        public override void OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease)
        {
            _lastInput ??= mouseRightRelease;
        }

        public override IPrediction Predict(Character character, MouseRightRelease rightRelease)
        {
            var next = character.Coordinate.Move(character.Direction);
            return new SetPositionPrediction(rightRelease, next, character.Direction);
        }

        public override void OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion)
        {
            _lastInput ??= mousePressedMotion;
        }

        public override IPrediction Predict(Character character, RightMousePressedMotion mousePressedMotion)
        {
            return NextMovePredict(character, mousePressedMotion);
        }
    }
}