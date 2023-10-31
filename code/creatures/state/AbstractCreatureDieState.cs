using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures.state
{
    public class AbstractCreatureDieState : AbstractCreatureState
    {
        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new()
        {
            { Direction.UP, 15},
			{ Direction.UP_RIGHT, 38},
			{ Direction.RIGHT, 61},
			{ Direction.DOWN_RIGHT, 84},
			{ Direction.DOWN, 107},
			{ Direction.DOWN_LEFT, 130},
			{ Direction.LEFT, 153},
			{ Direction.UP_LEFT, 176},
        };
        public AbstractCreatureDieState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
            creature.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(3, 0.2f, Godot.Animation.LoopModeEnum.None));
        }

        public override State State => State.DIE;

        public override int GetSpriteOffset()
        {
            return SPRITE_OFFSET.GetValueOrDefault(Direction, 1);
        }

        public override void OnAnimationFinised()
        {
            Creature.QueueFree();
        }
    }
}