using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.entity.equipment;
using y1000.Source.Creature;
using AbstractCreature = y1000.code.creatures.AbstractCreature;

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