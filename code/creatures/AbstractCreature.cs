using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures.state;
using y1000.code.entity;
using y1000.code.player;
using y1000.code.util;

namespace y1000.code.creatures
{
    public abstract partial class AbstractCreature<S> : Node2D, ICreature<S> where S : ICreatureState
    {
        public float gravity = 0;

        private S? currentState;

        private static readonly CreatureAnimationPlayer NULL_PLAYER = new();

        private static readonly Rect2I EMPTY = new (0, 0, 0, 0);

        private CreatureAnimationPlayer animationPlayer;

        public Point coordinate;

        protected AbstractCreature()
        {
            animationPlayer = NULL_PLAYER;
        }

        public Point Coordinate
        {
            get
            {
                return coordinate;
            }
            set
            {
                coordinate = value;
                Position = coordinate.CoordinateToPixel();
            }
        }

        protected void Setup()
        {
            SetMeta("spriteNumber", 0);
            animationPlayer = GetNode<CreatureAnimationPlayer>("AnimationPlayer");
            animationPlayer.AnimationFinished += OnAnimationFinised;
        }

        internal void ChangeState(S newState)
        {
            currentState = newState;
            currentState.PlayAnimation();
        }

        public Direction Direction => CurrentState.Direction;

        public OffsetTexture BodyTexture => CurrentState.OffsetTexture((int)GetMeta("spriteNumber"));

        public void Move(Direction direction)
        {
            CurrentState.Move(direction);
        }

        public S CurrentState
        {
            get
            {
                if (currentState != null)
                {
                    return currentState;
                }
                throw new NotImplementedException();
            }
            set
            {
                currentState = value;
            }
        }

        public abstract long Id { get; }

        public CreatureAnimationPlayer AnimationPlayer => animationPlayer;

        public override void _Process(double delta)
        {
            CurrentState.Process(delta);
        }

        protected void OnAnimationFinised(StringName name)
        {
            CurrentState.OnAnimationFinised();
        }

        public void Turn(Direction newDirection)
        {
            CurrentState.Turn(newDirection);
        }

        public void Attack()
        {
            CurrentState.Attack();
        }

        public void Hurt()
        {
            CurrentState.Hurt();
        }

        public void Die()
        {
            CurrentState.Die();
        }

        public Rect2I HoverRect()
        {
            foreach (var child in GetChildren())
            {
                GD.Print("Checking on " + child);
                if (child is ICreatureBodySprite creatureBody)
                {
                    return creatureBody.HoverRect();
                }
            }
            GD.Print("Got emtpy.");
            return EMPTY;
        }


        public void Remove()
        {
            QueueFree();
        }
    }
}