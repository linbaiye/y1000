using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.code.creatures.state
{
    public sealed class SimpleCreatureDieState : AbstractCreatureDieState
    {
        public SimpleCreatureDieState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
        }

        protected override SpriteReader SpriteReader => ((SimpleCreature)Creature).SpriteReader;
    }
}