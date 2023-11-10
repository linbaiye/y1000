using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public class HurtState : AbstractPlayerState
    {
        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new Dictionary<Direction, int>()
        {
            { Direction.UP, 184},
            { Direction.UP_RIGHT, 188},
            { Direction.RIGHT, 192},
            { Direction.DOWN_RIGHT, 196},
            { Direction.DOWN, 200},
            { Direction.DOWN_LEFT, 204},
            { Direction.LEFT, 208},
            { Direction.UP_LEFT, 212},
        };

        public override PositionedTexture BodyTexture => SpriteContainer.LoadMaleCharacterSprites("N02").Get(SPRITE_OFFSET.GetValueOrDefault(Direction) + Character.PictureNumber);

        private readonly IPlayerState previousState;

        public HurtState(Character character, Direction direction, IPlayerState playerState) : base(character, direction)
        {
            previousState = playerState;
            if (!Character.AnimationPlayer.HasAnimationLibrary(State.ToString()))
            {
                character.AnimationPlayer.AddAnimationLibrary(State.ToString(), CreateAnimations(4, 0.08f));
            }
            character.AnimationPlayer.Play(State + "/" + Direction);
        }


        public override void OnAnimationFinished(StringName name)
        {
            Character.ChangeState(new IdleState(Character, Direction));
        }

        public override void Process(double delta)
        {
        }

        public override State State => State.HURT;

        public override PositionedTexture HandTexture => throw new NotImplementedException();
    }
}