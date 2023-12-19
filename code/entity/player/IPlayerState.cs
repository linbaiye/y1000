using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.player;

namespace y1000.code.entity.player
{
    public interface IPlayerState
    {
        void PlayAnimation(Player player, StateSegment segment);

        ChestSprite? ChestSprite();
    }
}