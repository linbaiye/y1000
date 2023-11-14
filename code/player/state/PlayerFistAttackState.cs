using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;

namespace y1000.code.player.state
{
    public class PlayerFistAttackState : AbstractPlayerAttackState
    {
        private static readonly Dictionary<int, Dictionary<Direction, int>> UNARMED_SPRITE_OFFSET = new Dictionary<int, Dictionary<Direction, int>>()
        {
            {0, new Dictionary<Direction, int>()
            {
            { Direction.UP, 0},
            { Direction.UP_RIGHT, 7},
            { Direction.RIGHT, 14},
            { Direction.DOWN_RIGHT, 21},
            { Direction.DOWN, 28},
            { Direction.DOWN_LEFT, 35},
            { Direction.LEFT, 42},
            { Direction.UP_LEFT, 49},
            }},

            {1, new Dictionary<Direction, int>()
            {
            { Direction.UP, 56},
            { Direction.UP_RIGHT, 61},
            { Direction.RIGHT, 66},
            { Direction.DOWN_RIGHT, 71},
            { Direction.DOWN, 76},
            { Direction.DOWN_LEFT, 81},
            { Direction.LEFT, 86},
            { Direction.UP_LEFT, 91},
            }}
        };

        public PlayerFistAttackState(AbstractCreature creature, Direction direction, AbstractCreatureStateFactory _stateFactory) : base(creature, direction, _stateFactory)
        {
            
        }

        public PlayerFistAttackState(Player player, Direction direction) : this(player, direction, PlayerStateFactory.INSTANCE)
        {
        }

        protected override int SpriteOffset => 

        protected override SpriteContainer SpriteContainer => throw new NotImplementedException();

        public override OffsetTexture ChestTexture(int animationSpriteNumber, IChestArmor armor)
        {
            throw new NotImplementedException();
        }
    }
}