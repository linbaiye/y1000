using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures.state;
using y1000.code.player;
using y1000.code.player.state;

namespace y1000.code.character.state
{
    public class CharacterKickAttackState : PlayerKickAttackState
    {
        public CharacterKickAttackState(Player player, Direction direction) : base(player, direction, CharacterStateFactory.INSTANCE)
        {
        }
    }
}