using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;

namespace y1000.code.player.state
{
    public class PlayerIdleState : AbstractCreatureIdleState , IPlayerState
    {

        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new ()
        {
            { Direction.UP, 48},
			{ Direction.UP_RIGHT, 51},
			{ Direction.RIGHT, 54},
			{ Direction.DOWN_RIGHT, 57},
			{ Direction.DOWN, 60},
			{ Direction.DOWN_LEFT, 63},
			{ Direction.LEFT, 66},
			{ Direction.UP_LEFT, 69},
        };

        public PlayerIdleState(Player player, Direction direction) : base(player, direction, SPRITE_OFFSET, 3, 0.5f, PlayerStateFactory.INSTANCE)
        {
        }


        public PlayerIdleState(Player player, Direction direction, AbstractCreatureStateFactory stateFactory) : base(player, direction, SPRITE_OFFSET, 3, 0.5f, stateFactory)
        {
        }

        public SpriteContainer SpriteContainer => ((Player)Creature).IsMale() ? SpriteContainer.LoadMaleCharacterSprites("N02"): SpriteContainer.EmptyContainer;
    }
}