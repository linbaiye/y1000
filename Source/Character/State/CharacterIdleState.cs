using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using y1000.code;
using y1000.code.character.state;
using y1000.code.character.state.input;
using y1000.code.character.state.Prediction;
using y1000.code.player;
using y1000.code.util;

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

        public override void OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            character.Direction = rightClick.Direction;
            var nextCoor = character.Coordinate.Move(rightClick.Direction);
            if (character.Realm.CanMove(nextCoor))
            {
                character.ChangeState(CharacterMoveState.Create(character.IsMale));
            }
            else
            {
                character.ChangeState(Create(character.IsMale));
            }
        }

        public override IPrediction Predict(Character character, MouseRightClick rightClick)
        {
            var nextCoor = character.Coordinate.Move(rightClick.Direction);
            if (character.Realm.CanMove(nextCoor))
            {
                return new MovedPrediction(rightClick, character.Coordinate);
            }
            else
            {
                return new IdlePrediction(rightClick, nextCoor);
            }
        }

        public override void Process(Character character, long deltaMillis)
        {
            ElpasedMillis += deltaMillis;
        }

        public override bool RespondsTo(IInput input)
        {
            return input is MouseRightClick;
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
    }
}