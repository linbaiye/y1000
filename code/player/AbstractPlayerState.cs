using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public abstract class AbstractPlayerState : IPlayerState
    {

        private readonly Character character;

        public Direction Direction {get; set;}

        protected AbstractPlayerState(Character character, Direction direction)
        {
            this.character = character;
            Direction = direction;
        }

        protected AbstractPlayerState(Character character)
        {
            this.character = character;
        }


        public abstract State State { get; }
        public abstract PositionedTexture BodyTexture { get; }

        public abstract void PhysicsProcess(double delta);


        protected AnimationLibrary CreateAnimations(int total, float step)
        {
            return CreateAnimations(total, step, Animation.LoopModeEnum.None);
        }


        protected AnimationLibrary CreateAnimations(int total, float step, Animation.LoopModeEnum loopModeEnum)
        {
            AnimationLibrary library = new AnimationLibrary();
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var animation = CreateAnimation(total, step);
                animation.LoopMode = loopModeEnum;
                library.AddAnimation(direction.ToString(), animation);
            }
            return library;
        }


        protected static Animation CreateAnimation(int total, float step)
        {
            Animation animation = new Animation();
            int trackIdx = animation.AddTrack(Animation.TrackType.Value);
            animation.TrackSetPath(trackIdx, ".:metadata/picNumber");
            float time = 0.0f;
            //animation.Step = step;
            for (int i = 0; i <= total; i++, time += step)
            {
                animation.TrackInsertKey(trackIdx, time, Math.Min(i, total - 1));
            }
            animation.Length = total * step;
            return animation;
        }

        public virtual void OnAnimationFinished(StringName animationName) => new NotImplementedException();

        public Character Character => character;
    }
}