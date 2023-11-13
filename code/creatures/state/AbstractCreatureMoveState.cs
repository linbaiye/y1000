using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.character.state;
using y1000.code.util;

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

        private readonly Dictionary<Direction, int> spriteOffset;

        private const int SPRITE_NUMBERS = 7;

        private Point nextCoordinate;

        private readonly int spriteNumber;

        private readonly float step;


        public AbstractCreatureMoveState(AbstractCreature creature, Direction direction) : this(creature, direction, SPRITE_OFFSET, SPRITE_NUMBERS, 0.1f, SimpleCreatureStateFactory.INSTANCE)
        {
            nextCoordinate = creature.Coordinate.Next(direction);
            spriteNumber = SPRITE_NUMBERS;
            step = 0.1f;
        }

        public AbstractCreatureMoveState(AbstractCreature creature, Direction direction, 
            Dictionary<Direction, int> _spriteOffset, int spriteNumber, float step, AbstractCreatureStateFactory abstractCreatureStateFactory) : base(creature, direction, abstractCreatureStateFactory)
        {
            velocity = VectorUtil.Velocity(direction) / (step * spriteNumber);
            creature.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(spriteNumber, step, Animation.LoopModeEnum.None));
            spriteOffset = _spriteOffset;
            nextCoordinate = creature.Coordinate.Next(direction);
            this.spriteNumber = spriteNumber;
            this.step = step;
        }

        protected override int SpriteOffset => spriteOffset.GetValueOrDefault(Direction, -1);

        public void Process(double delta)
        {
            Creature.Position += velocity * (float) delta;
        }

        protected void ChangeCoordinate()
        {
            Creature.Coordinate = nextCoordinate;
        }


        protected void MoveTo(Direction direction, Point next)
        {
            velocity = VectorUtil.Velocity(direction) / (spriteNumber * step);
            nextCoordinate = next;
            SetDirection(direction);
        }

        public override void Hurt()
        {
            StopAndChangeState(StateFactory.CreateHurtState(Creature));
        }
    }
}