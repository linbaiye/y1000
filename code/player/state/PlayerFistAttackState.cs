using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;

namespace y1000.code.player.state
{
    public class PlayerFistAttackState : AbstractPlayerUnarmedAttackState
    {
        private static readonly Dictionary<Direction, int> BELOW_50_SPRITE = new Dictionary<Direction, int>()
        {
            { Direction.UP, 55},
            { Direction.UP_RIGHT, 60},
            { Direction.RIGHT, 65},
            { Direction.DOWN_RIGHT, 70},
            { Direction.DOWN, 75},
            { Direction.DOWN_LEFT, 80},
            { Direction.LEFT, 85},
            { Direction.UP_LEFT, 90},
        };

        public PlayerFistAttackState(Player player, Direction direction, AbstractCreatureStateFactory _stateFactory) : base(player, direction, _stateFactory, BELOW_50_SPRITE)
        {
            player.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(5, 0.1f, Animation.LoopModeEnum.Linear));
        }

        public PlayerFistAttackState(Player player, Direction direction) : this(player, direction, PlayerStateFactory.INSTANCE)
        {
        }
    }
}