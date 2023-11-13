using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.creatures.state
{
    public abstract class AbstractCreatureIdleState : AbstractCreatureState
    {
        private static readonly Dictionary<Direction, int> DEFAULT_SPRITE_OFFSET = new()
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


        private readonly Dictionary<Direction, int> spriteOffset;

        public AbstractCreatureIdleState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
            creature.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(5, 0.25f, Godot.Animation.LoopModeEnum.Linear));
            spriteOffset = DEFAULT_SPRITE_OFFSET;
        }

        public AbstractCreatureIdleState(AbstractCreature creature, Direction direction, Dictionary<Direction, int> _spriteOffset,
        int totalSrpite, float step, AbstractCreatureStateFactory stateFactory) :
         base(creature, direction, stateFactory)
        {
            creature.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(totalSrpite, step, Godot.Animation.LoopModeEnum.Linear));
            spriteOffset = _spriteOffset;
        }


        public override State State => State.IDLE;

        protected override int SpriteOffset => spriteOffset.GetValueOrDefault(Direction, -1);

        public override void Move(Direction direction)
        {
            StopAndChangeState(StateFactory.CreateMoveState(Creature, direction));
        }

        public override void Turn(Direction newDirection)
        {
            SetDirection(newDirection);
            Creature.AnimationPlayer.Play(State  + "/" + Direction);
        }

        public override void Attack()
        {
            StopAndChangeState(StateFactory.CreateAttackState(Creature));
        }

        public override void Hurt()
        {
            StopAndChangeState(StateFactory.CreateHurtState(Creature));
        }

    }
}