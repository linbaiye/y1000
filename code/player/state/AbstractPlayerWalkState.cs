using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.Source.Sprite;

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
        //public const float STEP = 0.065f;
        public const float STEP = 0.15f;

        public AbstractPlayerWalkState(Player _player, Direction direction) : base(_player, direction, SPRITE_OFFSET, SPRITE_NUMBER, STEP, PlayerStateFactory.INSTANCE, 1.0f)
        {
        }

        public AbstractPlayerWalkState(Player _player, Direction direction, AbstractCreatureStateFactory stateFactory) : base(_player, direction, SPRITE_OFFSET, SPRITE_NUMBER, STEP, stateFactory, 1.0f)
        {
        }

        protected override SpriteReader SpriteReader => ((Player)Creature).IsMale() ? SpriteReader.LoadMalePlayerSprites("N02") : SpriteReader.EmptyReader;
    
    }
}