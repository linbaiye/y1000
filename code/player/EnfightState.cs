using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;

namespace y1000.code.player
{
    public class EnfightState : AbstractPlayerState
    {
        public EnfightState(Character character, Direction direction) : base(character, direction)
        {
            if (!character.AnimationPlayer.HasAnimationLibrary(State.ToString()))
            {
                character.AnimationPlayer.AddAnimationLibrary(State.ToString(), CreateAnimations(3, 0.5f, Animation.LoopModeEnum.Linear));
            }
            character.AnimationPlayer.Play(State + "/" + Direction);
        }

        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new Dictionary<Direction, int>()
        {

			{ Direction.UP, 120},
			{ Direction.UP_RIGHT, 123},
			{ Direction.RIGHT, 126},
			{ Direction.DOWN_RIGHT, 129},
			{ Direction.DOWN, 132},
			{ Direction.DOWN_LEFT, 135},
			{ Direction.LEFT, 138},
			{ Direction.UP_LEFT, 141},

        };

        public override PositionedTexture BodyTexture => SpriteContainer.LoadMaleCharacterSprites("N02").Get(SPRITE_OFFSET.GetValueOrDefault(Direction) + Character.PictureNumber);


        public EnfightState(Character character) : base(character)
        {
        }

        public override State State => State.ENFIGHT;

        public override PositionedTexture HandTexture => throw new NotImplementedException();


        public override void RightMousePressed(Vector2 mousePosition)
        {
            Direction = ComputeDirection(mousePosition);
        }

        public override void Attack()
        {
            Character.ChangeState(new AttackingState(Character, Direction));
        }

        public override void Attack(ICreature target)
        {
            Character.ChangeState(new AttackingState(Character, Direction, target));
        }

        public override void Process(double delta)
        {
        }
    }
}