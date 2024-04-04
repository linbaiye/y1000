using System;
using System.Collections.Generic;
using y1000.code;
using y1000.code.character.state;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;

namespace y1000.Source.Character.State
{
    public class CharacterIdleState : AbstractCharacterState
    {

        private static readonly Dictionary<Direction, int> BODY_SPRITE_OFFSET = new()
        {
            { Direction.UP, 48},
			{ Direction.UP_RIGHT, 51},
			{ Direction.RIGHT, 54},
			{ Direction.DOWN_RIGHT, 57},
			{ Direction.DOWN, 60},
			{ Direction.DOWN_LEFT, 63},
			{ Direction.LEFT, 66},
			{ Direction.UP_LEFT, 69},
        };

        public CharacterIdleState(AnimatedSpriteManager spriteManager): base(spriteManager)
        {
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
                character.ChangeState(CharacterMoveState.Create(character.IsMale, input));
                return new CharacterMoveEvent()
            }
            else
            {
                character.Direction = input.Direction;
                character.ChangeState(Create(character.IsMale));
            }
        }

        public override IClientEvent OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            MoveByClick(character, rightClick);
        }

        public override IPrediction Predict(Character character, MouseRightClick rightClick)
        {
            return PredictRightClick(character, rightClick);
        }

        public override void Process(Character character, long deltaMillis)
        {
            ElpasedMillis += deltaMillis;
        }

        public override bool CanHandle(IInput input)
        {
            return input is AbstractRightClickInput;
        }

        public static CharacterIdleState ForMale()
        {
            var container = SpriteContainer.LoadMalePlayerSprites("N02");
            var sm = AnimatedSpriteManager.WithPinpong(500, BODY_SPRITE_OFFSET, container);
            return new CharacterIdleState(sm);
        }

        public static CharacterIdleState Create(bool forMale)
        {
            return ForMale();
        }

        public override void OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease)
        {
            throw new NotImplementedException();
        }

        public override IPrediction Predict(Character character, MouseRightRelease release)
        {
            throw new NotImplementedException();
        }

        public override void OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion)
        {
            MoveByClick(character, mousePressedMotion);
        }

        public override IPrediction Predict(Character character, RightMousePressedMotion mousePressedMotion)
        {
            return PredictRightClick(character, mousePressedMotion);
        }
    }
}