using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.creatures.state
{
    public abstract class AbstractCreatureMoveState : AbstractCreatureState
    {
        private Vector2 velocity;

        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new()
        {
            { Direction.UP, 0},
			{ Direction.UP_RIGHT, 23},
			{ Direction.RIGHT, 46},
			{ Direction.DOWN_RIGHT, 69},
			{ Direction.DOWN, 92},
			{ Direction.DOWN_LEFT, 115},
			{ Direction.LEFT, 138},
			{ Direction.UP_LEFT, 161},
        };

        private const int SPRITE_NUMBERS = 6;

        public AbstractCreatureMoveState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
            velocity = VectorUtil.Velocity(direction) / 0.6f;
            creature.AddAnimationLibrary(State.ToString(), () => AnimationUtil.CreateAnimations(SPRITE_NUMBERS, 0.1f, Animation.LoopModeEnum.None));
            creature.AnimationPlayer.Play(State + "/" + Direction);
        }

        public override State State => State.MOVE;

        public override int GetSpriteOffset()
        {
            return SPRITE_OFFSET.GetValueOrDefault(Direction, -1);
        }

        public void PhysicsProcess(double delta)
        {
            Creature.MoveAndCollide(velocity * (float) delta);
        }

        protected abstract ICreatureState CreateIdleState();

        public override void OnAnimationFinised()
        {
            Creature.Position = Creature.Position.Snapped(VectorUtil.TILE_SIZE);
            Creature.ChangeState(CreateIdleState());
        }

        public override void ChangeDirection(Direction newDirection)
        {
        }
    }
}