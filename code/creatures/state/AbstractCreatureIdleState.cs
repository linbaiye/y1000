using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures.state
{
    public abstract class AbstractCreatureIdleState : AbstractCreatureState
    {
        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new()
        {
            { Direction.UP, 18},
			{ Direction.UP_RIGHT, 41},
			{ Direction.RIGHT, 64},
			{ Direction.DOWN_RIGHT, 87},
			{ Direction.DOWN, 110},
			{ Direction.DOWN_LEFT, 133},
			{ Direction.LEFT, 156},
			{ Direction.UP_LEFT, 179},
        };

        public AbstractCreatureIdleState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
            creature.AddAnimationLibrary(State.ToString(), () => AnimationUtil.CreateAnimations(5, 0.5f, Godot.Animation.LoopModeEnum.Linear));
		    creature.AnimationPlayer.Play(State + "/" + Direction);
        }

        public override State State => State.IDLE;

        public override int GetSpriteOffset()
        {
            return SPRITE_OFFSET.GetValueOrDefault(Direction, -1);
        }

        protected abstract ICreatureState CreateMoveState(Direction newDirection);

        protected abstract ICreatureState CreateAttackState();

        public override void Move(Direction direction)
        {
            Creature.AnimationPlayer.Stop();
            Creature.ChangeState(CreateMoveState(direction));
        }

        public override void ChangeDirection(Direction newDirection)
        {
            Creature.AnimationPlayer.Stop();
            SetDirection(newDirection);
            Creature.AnimationPlayer.Play(State  + "/" + Direction);
        }

        public override void Attack()
        {
            Creature.AnimationPlayer.Stop();
            Creature.ChangeState(CreateAttackState());
        }
    }
}