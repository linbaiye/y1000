using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.Source.Creature;

namespace y1000.code.creatures.state
{
    public abstract class AbstractCreatureDieState : AbstractCreatureState
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

        protected override int SpriteOffset => SPRITE_OFFSET.GetValueOrDefault(Direction, -1);

        public override CreatureState State => CreatureState.DIE;
    }
}