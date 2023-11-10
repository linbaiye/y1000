using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.player.state;

namespace y1000.code.character.state
{
    public sealed class CharacterIdleState : AbstractPlayerIdleState
    {
        public CharacterIdleState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
        }
    }
}