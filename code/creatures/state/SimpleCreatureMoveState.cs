using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures.state
{
    public class SimpleCreatureMoveState : AbstractCreatureMoveState
    {
        public SimpleCreatureMoveState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {

        }

        protected override SpriteContainer SpriteContainer => ((SimpleCreature)Creature).SpriteContainer;

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