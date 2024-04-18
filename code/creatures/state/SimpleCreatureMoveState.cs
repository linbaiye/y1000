using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.Source.Sprite;

namespace y1000.code.creatures.state
{
    public class SimpleCreatureMoveState : AbstractCreatureMoveState
    {
        public SimpleCreatureMoveState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {

        }

        protected override SpriteReader SpriteReader => ((SimpleCreature)Creature).SpriteReader;

        public override CreatureState State => CreatureState.WALK;

        public override void OnAnimationFinised()
        {
            UpdateCooridnate();
            StopAndChangeState(StateFactory.CreateIdleState(Creature));
        }

        public override void Hurt()
        {
            StopAndChangeState(StateFactory.CreateHurtState(Creature));
        }
    }
}