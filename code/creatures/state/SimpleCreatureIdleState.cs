using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.player;

namespace y1000.code.creatures.state
{
    public class SimpleCreatureIdleState : AbstractCreatureIdleState
    {
        public SimpleCreatureIdleState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
        }

        protected override SpriteContainer SpriteContainer => ((SimpleCreature)Creature).SpriteContainer;
    }
}