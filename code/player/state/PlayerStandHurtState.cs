using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.entity.equipment;
using y1000.Source.Creature;
using AbstractCreature = y1000.code.creatures.AbstractCreature;

namespace y1000.code.player.state
{
    // Get hurt while standing.
    public class PlayerStandHurtState : AbstractPlayerHurtState
    {
        public PlayerStandHurtState(AbstractCreature creature, Direction direction,
            Action finished) : base(creature, direction, finished)
        {
        }

        public override void OnAnimationFinised()
        {
            Creature.AnimationPlayer.Stop();
            InvokeCallback();
        }
    }
}