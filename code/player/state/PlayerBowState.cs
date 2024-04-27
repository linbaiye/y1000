using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures.state;
using y1000.Source.Creature;
using y1000.Source.Sprite;
using AbstractCreature = y1000.code.creatures.AbstractCreature;

namespace y1000.code.player.state
{
    public class PlayerBowState : AbstractCreatureState
    {
        public PlayerBowState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
        }

        public override CreatureState State => CreatureState.BOW;

        protected override int SpriteOffset => throw new NotImplementedException();

        protected override SpriteReader SpriteReader => throw new NotImplementedException();
    }
}