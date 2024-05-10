using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures.state;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.code.creatures
{
    public class UnknownState : AbstractCreatureState
    {
        public static readonly UnknownState INSTANCE = new UnknownState();

        private static readonly UnknowCreature UNKNOWN_CREATURE = new UnknowCreature();

        public UnknownState() : base(UNKNOWN_CREATURE, Direction.DOWN)
        {
        }

        public override CreatureState State => throw new NotImplementedException();


        protected override SpriteReader SpriteReader => throw new NotImplementedException();

        protected override int SpriteOffset => throw new NotImplementedException();
    }
}