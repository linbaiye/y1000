using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures
{
    public partial class UnknownState : AbstractCreatureState
    {
        public static readonly UnknownState INSTANCE = new UnknownState();

        private partial class UnknownCreature : AbstractCreature
        {

        }

        private static readonly UnknownCreature UNKNOWN_CREATURE = new UnknownCreature();

        public UnknownState() : base(UNKNOWN_CREATURE, Direction.DOWN)
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

        public override void Turn(Direction newDirection)
        {
            throw new NotImplementedException();
        }

        public override void Hurt()
        {
            throw new NotImplementedException();
        }
    }

}