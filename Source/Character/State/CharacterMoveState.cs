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

        private IInput? _input;

        public CharacterMoveState(AnimatedSpriteManager spriteManager) : base(spriteManager)
        {
        }

        public override bool RespondsTo(IInput input)
        {
            return input is RightMousePressedMotion || input is MouseRightRelease || input is MouseRightClick;
        }

        public override void OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            throw new NotImplementedException();
        }

        public override IPrediction Predict(Character character, MouseRightClick rightClick)
        {
            throw new NotImplementedException();
        }

        public override void Process(Character character, long deltaMillis)
        {
            ElpasedMillis += deltaMillis;
            int animationLengthMillis = SpriteManager.AnimationLength;
            var velocity = VectorUtil.Velocity(character.Direction);
            character.Position += velocity * ((float)deltaMillis / animationLengthMillis);
            if (ElpasedMillis <= animationLengthMillis)
            {
                return;
            }
            character.Position = character.Position.Snapped(VectorUtil.TILE_SIZE);
            if (_input is MouseRightRelease)
            {
                character.ChangeState(CharacterIdleState.Create(character.IsMale));
            }
            else
            {
                character.ChangeState(Create(character.IsMale));
            }
        }

        public static CharacterMoveState Create(bool forMale)
        {
            var asm = AnimatedSpriteManager.Normal(SPRITE_LENGTH_MILLIS, SPRITE_OFFSET, SpriteContainer.LoadMalePlayerSprites("N02"));
            return new CharacterMoveState(asm);
        }

        public override void OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease)
        {
            _input = mouseRightRelease;
        }

        public override IPrediction Predict(Character character, MouseRightRelease rightClick)
        {
            var next = character.Coordinate.Move(character.Direction);
            return new SetPositionPrediction(rightClick, next, character.Direction);
        }
    }
}