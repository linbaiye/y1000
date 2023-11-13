using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;

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
        public abstract OffsetTexture BodyTexture { get; }

        public abstract void Process(double delta);


        protected AnimationLibrary CreateAnimations(int total, float step)
        {
            return CreateAnimations(total, step, Animation.LoopModeEnum.None);
        }

                protected AnimationLibrary CreateAnimations(int total, float step, Action action)
        {
            return AnimationUtil.CreateAnimations(total, step, Animation.LoopModeEnum.None, action);
        }



        protected AnimationLibrary CreateAnimations(int total, float step, Animation.LoopModeEnum loopModeEnum)
        {
            return AnimationUtil.CreateAnimations(total, step, loopModeEnum);
        }


        protected static Animation CreateAnimation(int total, float step)
        {
            return AnimationUtil.CreateAnimation(total, step);
        }

        public virtual void OnAnimationFinished(StringName animationName) => new NotImplementedException();

        public virtual void RightMousePressed(Vector2 mousePosition)
        {
        }

        public virtual void Attack(ICreature target)
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

        public int GetSpriteOffset()
        {
            throw new NotImplementedException();
        }

        public void Move(Direction direction)
        {
            throw new NotImplementedException();
        }

        public void Turn(Direction newDirection)
        {
            throw new NotImplementedException();
        }

        public void OnAnimationFinised()
        {
            throw new NotImplementedException();
        }

        public void PlayAnimation()
        {
            Character.AnimationPlayer.Stop();
            Character.AnimationPlayer.Play(State + "/" + Direction);
        }

        public void Die()
        {
            throw new NotImplementedException();
        }

        public abstract OffsetTexture OffsetTexture(int animationSpriteNumber);

        public Character Character => (Character)character;

        public abstract OffsetTexture HandTexture { get; }

    }
}