using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.player;

namespace y1000.code.monsters
{
    public sealed class BuffaloIdleState : AbstractCreatureState
    {
        private Buffalo buffalo;

        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new Dictionary<Direction, int>()
        {
			{ Direction.UP, 18},
			{ Direction.UP_RIGHT, 23},
			{ Direction.RIGHT, 28},
			{ Direction.DOWN_RIGHT, 33},
			{ Direction.DOWN, 38},
			{ Direction.DOWN_LEFT, 43},
			{ Direction.LEFT, 48},
			{ Direction.UP_LEFT, 53},
        };

        public BuffaloIdleState(Buffalo buffalo, Direction direction) : base(direction)
        {
            this.buffalo = buffalo;
            buffalo.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(5, 0.5f, Godot.Animation.LoopModeEnum.Linear));
		    buffalo.AnimationPlayer.Play(State + "/" + Direction);
        }

        public override State State => State.IDLE;

        public override void Move(Direction direction)
        {
        }


        public override int GetSpriteOffset()
        {
            return SPRITE_OFFSET.GetValueOrDefault(Direction, -1);
        }
    }
}