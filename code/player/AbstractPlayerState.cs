using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public abstract class AbstractPlayerState : IPlayerState
    {

        private readonly IPlayer character;

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
            return CreateAnimations(total, step, loopModeEnum);
        }


        protected static Animation CreateAnimation(int total, float step)
        {
            return AnimationUtil.CreateAnimation(total, step);
        }

        public virtual void OnAnimationFinished(StringName animationName) => new NotImplementedException();

        public virtual void RightMousePressed(Vector2 mousePosition)
        {
        }

        public virtual void RightMouseRleased()
        {
        }

        public virtual void Attack()
        {
        }

        public virtual void Sit()
        {

        }

        public virtual void Hurt()
        {

        }

        protected static Direction ComputeDirection(Vector2 mousePosition)
        {
            var angle = Mathf.Snapped(mousePosition.Angle(), Mathf.Pi / 4) / (Mathf.Pi / 4);
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



        public Character Character => (Character)character;
    }
}