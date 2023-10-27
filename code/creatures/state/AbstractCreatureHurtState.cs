using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures.state
{
    public abstract class AbstractCreatureHurtState : AbstractCreatureState
    {
        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new()
        {
            { Direction.UP, 12},
			{ Direction.UP_RIGHT, 35},
			{ Direction.RIGHT, 58},
			{ Direction.DOWN_RIGHT, 81},
			{ Direction.DOWN, 104},
			{ Direction.DOWN_LEFT, 127},
			{ Direction.LEFT, 150},
			{ Direction.UP_LEFT, 173},
        };

        public AbstractCreatureHurtState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
            creature.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(3, 0.1f, Godot.Animation.LoopModeEnum.None));
        }

        public override State State => State.HURT;

        public override int GetSpriteOffset()
        {
            return SPRITE_OFFSET.GetValueOrDefault(Direction, -1);
        }

        public override void OnAnimationFinised()
        {
            Creature.ChangeState(StateFactory.CreatureDieState(Creature));
        }
    }
}