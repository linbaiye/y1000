using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures.state;
using y1000.code.player;

namespace y1000.code.creatures
{
    public abstract class AbstractCreatureState : ICreatureState
    {
        private Direction direction;

        private readonly AbstractCreature creature;

        private readonly AbstractCreatureStateFactory stateFactory;

        protected AbstractCreatureState(AbstractCreature creature, Direction direction)
        {
            this.creature = creature;
            this.direction = direction;
            stateFactory = new SimpleCreatureStateFactory();
        }

        protected AbstractCreature Creature => creature;

        public Direction Direction => direction;

        public abstract State State { get; }

        protected AbstractCreatureStateFactory StateFactory => stateFactory;

        public abstract int GetSpriteOffset();

        public virtual void Move(Direction direction)
        {

        }

        public void PlayAnimation()
        {
            Creature.AnimationPlayer.Play(State + "/" + Direction);
        }

        protected void StopAndChangeState(AbstractCreatureState newState)
        {
            creature.AnimationPlayer.Stop();
            creature.ChangeState(newState);
        }


        protected void SetDirection(Direction newDirection)
        {
            direction = newDirection;
        }


        public virtual void Turn(Direction newDirection) {}

        public virtual void OnAnimationFinised() {}

        public virtual void Attack() { }

        public virtual void Hurt() { }

        public virtual void Die()
        {
            if (State.DIE != State)
                StopAndChangeState(StateFactory.CreateDieState(Creature));
        }
    }
}