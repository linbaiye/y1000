using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.character.state;
using y1000.code.util;
using y1000.Source.Creature;
using y1000.Source.Util;

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

        private readonly int totalSpriteNumber;

        private readonly float step;

        private float speed;


        public AbstractCreatureMoveState(AbstractCreature creature, Direction direction) : this(creature, direction, SPRITE_OFFSET, SPRITE_NUMBERS, 0.1f, SimpleCreatureStateFactory.INSTANCE, 1.0f)
        {
        }

        public AbstractCreatureMoveState(AbstractCreature creature, Direction direction, 
            Dictionary<Direction, int> _spriteOffset, int totalSpriteNumber, float step, AbstractCreatureStateFactory abstractCreatureStateFactory, float speed) : base(creature, direction, abstractCreatureStateFactory)
        {
            creature.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(totalSpriteNumber, step, Animation.LoopModeEnum.None));
            spriteOffset = _spriteOffset;
            nextCoordinate = creature.Coordinate.Next(direction);
            this.totalSpriteNumber = totalSpriteNumber;
            this.step = step;
            ChangeSpeed(speed);
            ComputeVelocity();
        }

        protected override int SpriteOffset => spriteOffset.GetValueOrDefault(Direction, -1);

        public void Process(double delta)
        {
            Creature.Position += velocity * (float) delta;
            //Creature.Position = Creature.Coordinate.CoordinateToPixel() + (float)Creature.AnimationPlayer.CurrentAnimationPosition * velocity;
        }

        protected void UpdateCooridnate()
        {
            Creature.Coordinate = nextCoordinate;
        }

        private void ComputeVelocity()
        {
            velocity = VectorUtil.Velocity(Direction) / (step * totalSpriteNumber) * speed;
            //velocity = VectorUtil.Velocity(Direction) / (step * spriteNumber);
        }

        protected void ChangeSpeed(float newSpeed)
        {
            if (newSpeed > 0)
            {
                speed = newSpeed;
                Creature.AnimationPlayer.SpeedScale = speed;
            }
        }


        protected void ReadyToMoveTo(Direction direction, Point next)
        {
            SetDirection(direction);
            ComputeVelocity();
            nextCoordinate = next;
        }
    }
}