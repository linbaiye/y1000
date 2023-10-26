using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures
{
    public class UnknownState : AbstractCreatureState
    {
        public static readonly UnknownState INSTANCE = new UnknownState();
        private UnknownState() : base(Direction.DOWN)
        {
        }

        public override State State => throw new NotImplementedException();

        public override int GetSpriteOffset()
        {
            throw new NotImplementedException();
        }

        public override void Move(Direction direction)
        {
            throw new NotImplementedException();
        }
    }
}