using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public class WalkState : AbstractPlayerState
    {
        public WalkState(Character character, Direction direction) : base(character, direction)
        {
            if (!character.AnimationPlayer.HasAnimationLibrary(State.ToString())) {
                character.AnimationPlayer.AddAnimationLibrary(State.ToString(), CreateAnimations(6, 0.1f));
            }
            character.AnimationPlayer.Play(State + "/" + direction);
        }


        public override void OnAnimationFinished(StringName animationName)
        {
            Character.ChangeState(new IdleState(Character, Direction));
        }

        public override State State => State.WALK;

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

        public override PositionedTexture BodyTexture => SpriteContainer.LoadMaleCharacterSprites("N02").Get(SPRITE_OFFSET.GetValueOrDefault(Direction) + Character.PictureNumber);


        private Direction ComputeDirection()
        {
            var clickPosition = Character.GetLocalMousePosition();
            var angle = Mathf.Snapped(clickPosition.Angle(), Mathf.Pi / 4) / (Mathf.Pi / 4);
            int dir = Mathf.Wrap((int)angle, 0, 8);
            return dir switch
            {
                0 => Direction.RIGHT,
                1 => Direction.DOWN_RIGHT,
                2 => Direction.DOWN,
                3 => Direction.DOWN_LEFT,
                4 => Direction.LEFT,
                5 => Direction.UP_LEFT,
                6 => Direction.UP,
                7 => Direction.UP_RIGHT,
                _ => throw new NotSupportedException(),
            };
        }


        private Vector2 ComputeVelocity(Direction direction)
        {
            return direction switch
            {
                Direction.UP => new Vector2(0, -40),
                Direction.DOWN => new Vector2(0, 40),
                Direction.RIGHT => new Vector2(54, 0),
                Direction.LEFT => new Vector2(-54, 0),
                Direction.DOWN_LEFT => new Vector2(-54, 40),
                Direction.DOWN_RIGHT => new Vector2(54, 40),
                Direction.UP_LEFT => new Vector2(-54, -40),
                Direction.UP_RIGHT => new Vector2(54, -40),
                _ => Vector2.Zero,
            };
        }


        public override void PhysicsProcess(double delta)
        {
            if (Input.IsActionPressed("mouse_right"))
            {
                var player = Character.AnimationPlayer;
                Character.Velocity = ComputeVelocity(Direction);
                Direction = ComputeDirection();
                player.GetAnimation(player.CurrentAnimation).LoopMode = Animation.LoopModeEnum.Linear;
            }
            else if (Input.IsActionJustReleased("mouse_right"))
            {
                var player = Character.AnimationPlayer;
                player.GetAnimation(player.CurrentAnimation).LoopMode = Animation.LoopModeEnum.None;
            }
            Character.MoveAndSlide();
        }
    }
}