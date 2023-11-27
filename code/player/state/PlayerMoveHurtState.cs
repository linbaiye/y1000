using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
using y1000.code.entity.equipment;

namespace y1000.code.player.state
{
    public class PlayerMoveHurtState : AbstractPlayerHurtState
    {

        public PlayerMoveHurtState(AbstractCreature creature, Direction direction, Action callback) : base(creature, direction, callback)
        {
        }

        public override void OnAnimationFinised()
        {
            InvokeCallback();
        }
    }
}