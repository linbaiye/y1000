using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;

namespace y1000.code.player
{
    public class IdleState : AbstractPlayerState
    {
        public override State State => State.IDLE;


        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new Dictionary<Direction, int>()
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

        private static readonly Dictionary<Direction, int> WEAPON_SPRITE_OFFSET = new Dictionary<Direction, int>()
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


        public override PositionedTexture HandTexture => throw new NotImplementedException();

        public override PositionedTexture BodyTexture => SpriteContainer.LoadMaleCharacterSprites("N02").Get(SPRITE_OFFSET.GetValueOrDefault(Direction));



        public IdleState(Character character, Direction direction) : base(character, direction)
        {
            if (!character.AnimationPlayer.HasAnimationLibrary(State.ToString()))
            {
                var animationLibrary = CreateAnimations(3, 0.5f, Animation.LoopModeEnum.Linear);
                character.AnimationPlayer.AddAnimationLibrary(State.ToString(), animationLibrary);
            }
            Character.AnimationPlayer.Play(State.ToString() + "/" + Direction.ToString());
        }


        public override void RightMousePressed(Vector2 mousePosition)
        {
            Character.ChangeState(new WalkState(Character, mousePosition));
        }

        public override void Attack()
        {
            Character.ChangeState(new AttackingState(Character, Direction));
        }

        public override void Process(double delta)
        {
            var po = Character.Coordinate;
            //Character.GetParent().GetNode<WorldMap>("MapLayer").NotifyCharPosition(new Vector2I(po.X, po.Y));
        }

        public override void Attack(ICreature target)
        {
            Character.ChangeState(new AttackingState(Character, Direction, target));
        }

        public override void Hurt()
        {
            Character.ChangeState(new HurtState(Character, Direction, this));
        }
    }
}