using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures.state
{
    public sealed class SimpleCreatureDieState : AbstractCreatureDieState
    {
        public SimpleCreatureDieState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
        }

        protected override SpriteContainer SpriteContainer => ((SimpleCreature)Creature).SpriteContainer;
    }
}