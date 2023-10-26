using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.player;

namespace y1000.code.creatures
{
    public abstract class AbstractCreatureState : ICreatureState
    {
        private Direction direction;

        public AbstractCreatureState(Direction direction)
        {
            this.direction = direction;
        }

        public Direction Direction => direction;

        public abstract State State { get; }

        public abstract int GetSpriteOffset();

        public abstract void Move(Direction direction);
    }
}