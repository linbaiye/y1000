using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures.state;
using y1000.code.player;

namespace y1000.code.creatures
{
    public abstract class AbstractCreatureState<C, S>: ICreatureState where C : ICreature<S> where S : ICreatureState
    {
        private Direction direction;

        private readonly C creature;

        private readonly AbstractCreatureStateFactory stateFactory;

        protected AbstractCreatureState(C creature, Direction direction)
        {
            this.creature = creature;
            this.direction = direction;
            stateFactory = new SimpleCreatureStateFactory();
        }

        protected AbstractCreatureState(C creature, Direction direction, AbstractCreatureStateFactory _stateFactory)
        {
            this.creature = creature;
            this.direction = direction;
            stateFactory = _stateFactory;
        }

        protected C Creature => creature;

        public Direction Direction => direction;

        public abstract State State { get; }

        protected AbstractCreatureStateFactory StateFactory => stateFactory;

        protected abstract int SpriteOffset { get; }

        protected abstract SpriteContainer SpriteContainer { get; }

        public virtual void Move(Direction direction)
        {

        }

        public void PlayAnimation()
        {
            Creature.AnimationPlayer.Play(State + "/" + Direction);
        }

        protected void StopAndChangeState(S newState)
        {
            creature.AnimationPlayer.Stop();
            creature.CurrentState = newState;
        }

        public virtual void Turn(Direction newDirection) 
        {
            direction = newDirection;
        }

        public virtual void OnAnimationFinised() {}

        public virtual void Attack() { }

        public virtual void Hurt() { }

        public virtual void Die()
        {
        }

        public OffsetTexture OffsetTexture(int animationSpriteNumber)
        {
            return SpriteContainer.Get(SpriteOffset + animationSpriteNumber);
        }
    }
}