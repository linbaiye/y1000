using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures.state
{
    public abstract class AbstractCreatureAttackState : AbstractCreatureState
    {
        protected AbstractCreatureAttackState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
            creature.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(5, 0.1f, Godot.Animation.LoopModeEnum.None));
        }

        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new Dictionary<Direction, int>()
        {
            {Direction.UP, 7},
            {Direction.UP_RIGHT, 30},
            {Direction.RIGHT, 53},
            {Direction.DOWN_RIGHT, 76},
			{Direction.DOWN, 99},
            {Direction.DOWN_LEFT, 122},
            {Direction.LEFT, 145},
            {Direction.UP_LEFT, 168}
        };

        public override State State => State.ATTACKING;

        public override void OnAnimationFinised()
        {
            StopAndChangeState(StateFactory.CreateIdleState(Creature));
        }

        public override int GetSpriteOffset()
        {
            return SPRITE_OFFSET.GetValueOrDefault(Direction, -1);
        }

        public override void Hurt()
        {
            StopAndChangeState(StateFactory.CreateHurtState(Creature));
        }
    }
}