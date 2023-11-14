using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;

namespace y1000.code.player.state
{
    public class PlayerKickAttackState : AbstractPlayerUnarmedAttackState
    {
        private static readonly Dictionary<Direction, int> ABOVE_50_SPRITE = new Dictionary<Direction, int>()
        {
            { Direction.UP, 0},
            { Direction.UP_RIGHT, 6},
            { Direction.RIGHT, 13},
            { Direction.DOWN_RIGHT, 20},
            { Direction.DOWN, 27},
            { Direction.DOWN_LEFT, 34},
            { Direction.LEFT, 41},
            { Direction.UP_LEFT, 48}
        };

 

        public PlayerKickAttackState(Player player, Direction direction, AbstractCreatureStateFactory _stateFactory) : base(player, direction, _stateFactory, ABOVE_50_SPRITE)
        {
            player.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(7, 0.1f, Animation.LoopModeEnum.Linear));
        }

        public PlayerKickAttackState(Player player, Direction direction) : this(player, direction, PlayerStateFactory.INSTANCE)
        {
        }

    }
}