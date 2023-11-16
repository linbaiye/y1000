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
        private readonly Vector2 velocity;

        public PlayerMoveHurtState(AbstractCreature creature, Direction direction, Action callback, Vector2 v) : base(creature, direction, callback)
        {
            velocity = v;
        }

        protected override string GetEquipmentSpritePath(IEquipment equipment)
        {
            return "0";
        }

        public void Process(double delta)
        {
            
        }
    }
}