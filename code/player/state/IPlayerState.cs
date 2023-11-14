using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.entity.equipment.chest;

namespace y1000.code.player.state
{
    public interface IPlayerState : ICreatureState
    {
        OffsetTexture ChestTexture(int animationSpriteNumber, IChestArmor armor);
    }
}