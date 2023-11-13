using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public class WalkState : AbstractPlayerState
    {

        private bool mouseRightPressed;


        private Vector2 nextPosition;

        public WalkState(Character character, Vector2 mousePosition) : base(character, ComputeDirection(mousePosition))
        {
            if (!character.AnimationPlayer.HasAnimationLibrary(State.ToString())) {
                character.AnimationPlayer.AddAnimationLibrary(State.ToString(), CreateAnimations(6, 0.1f));
            }
            PlayAnimation();
            mouseRightPressed = true;
            nextPosition = mousePosition;
        }



        public override void OnAnimationFinished(StringName animationName)
        {
            Character.Position = Character.Position.Snapped(VectorUtil.TILE_SIZE);
            if (!mouseRightPressed)
            {
                Character.ChangeState(new IdleState(Character, Direction));
            }
            else
            {
                Direction = ComputeDirection(nextPosition);
                PlayAnimation();
            }
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

        public override OffsetTexture BodyTexture => SpriteContainer.LoadMaleCharacterSprites("N02").Get(SPRITE_OFFSET.GetValueOrDefault(Direction) );

        public override OffsetTexture HandTexture => throw new NotImplementedException();

        private Vector2 ComputeVelocity()
        {
            return Direction switch
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



        public override void RightMouseRleased()
        {
            mouseRightPressed = false;
        }


        public override void RightMousePressed(Vector2 mousePosition)
        {
            mouseRightPressed = true;
            nextPosition = mousePosition;
        }


        public override void Process(double delta)
        {
            Character.Position += ComputeVelocity() * (float) delta;
        }

        public override OffsetTexture OffsetTexture(int animationSpriteNumber)
        {
            throw new NotImplementedException();
        }
    }
}