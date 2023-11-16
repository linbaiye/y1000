using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.entity.equipment;

namespace y1000.code.player.state
{
    // Get hurt while standing.
    public class PlayerStandHurtState : AbstractPlayerHurtState
    {
        public PlayerStandHurtState(AbstractCreature creature, Direction direction,
            Action finished) : base(creature, direction, finished)
        {
        }

         protected override string GetEquipmentSpritePath(IEquipment equipment)
        {
            return equipment.SpriteBasePath + "0";
        }
    }
}