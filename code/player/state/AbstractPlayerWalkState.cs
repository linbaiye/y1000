using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;

namespace y1000.code.player.state
{
    public abstract class AbstractPlayerWalkState : AbstractCreatureMoveState
    {
        public static readonly Dictionary<Direction, int> SPRITE_OFFSET = new Dictionary<Direction, int>()
        {
            { Direction.UP, 0},
            { Direction.UP_RIGHT, 6},
            { Direction.RIGHT, 12},
            { Direction.DOWN_RIGHT, 18},
            { Direction.DOWN, 24},
            { Direction.DOWN_LEFT, 30},
            { Direction.LEFT, 36},
            { Direction.UP_LEFT, 42},
        };

        public const int SPRITE_NUMBER = 6;
        public const float STEP = 0.13f;

        public AbstractPlayerWalkState(Player _player, Direction direction) : base(_player, direction, SPRITE_OFFSET, SPRITE_NUMBER, STEP, PlayerStateFactory.INSTANCE)
        {
        }

        public AbstractPlayerWalkState(Player _player, Direction direction, AbstractCreatureStateFactory stateFactory) : base(_player, direction, SPRITE_OFFSET, SPRITE_NUMBER, STEP, stateFactory)
        {
        }

        protected override SpriteContainer SpriteContainer => ((Player)Creature).IsMale() ? SpriteContainer.LoadMalePlayerSprites("N02") : SpriteContainer.EmptyContainer;
    }
}