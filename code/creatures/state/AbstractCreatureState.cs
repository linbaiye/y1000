using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.player;

namespace y1000.code.creatures
{
    public abstract class AbstractCreatureState : ICreatureState
    {
        private Direction direction;

        private readonly AbstractCreature creature;

        public AbstractCreatureState(AbstractCreature creature, Direction direction)
        {
            this.direction = direction;
            this.creature = creature;
        }

        protected AbstractCreature Creature => creature;

        public Direction Direction => direction;

        public abstract State State { get; }

        public abstract int GetSpriteOffset();

        public virtual void Move(Direction direction)
        {

        }

        protected void SetDirection(Direction newDirection)
        {
            direction = newDirection;
        }


        public abstract void ChangeDirection(Direction newDirection);

        public virtual void OnAnimationFinised() {}

        public virtual void Attack()
        {
        }

    }
}