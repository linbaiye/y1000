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
    public abstract partial class AbstractCreature : Node2D, ICreature
    {
        public float gravity = 0;

        private ICreatureState currentState = UnknownState.INSTANCE;

        private static readonly CreatureAnimationPlayer NULL_PLAYER = new();

        private static readonly Rect2I EMPTY = new (0, 0, 0, 0);

        private CreatureAnimationPlayer animationPlayer;

        public Point coordinate;

        private string name = "test";

        protected AbstractCreature()
        {
            animationPlayer = NULL_PLAYER;
        }
        
        public Point Coordinate {
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

        public string EntityName  => name;

        protected void SetupAnimationPlayer()
        {
            SetMeta("spriteNumber", 0);
            animationPlayer = GetNode<CreatureAnimationPlayer>("AnimationPlayer");
            animationPlayer.AnimationFinished += OnAnimationFinised;
        }

        internal void ChangeState(ICreatureState newState)
        {
            currentState = newState;
            currentState.PlayAnimation();
        }


        protected int SpriteNumber => (int)GetMeta("spriteNumber");

        public Direction Direction => currentState.Direction;

        public OffsetTexture BodyTexture => CurrentState.OffsetTexture(SpriteNumber);

        public void Move(Direction direction)
        {
            currentState.Move(direction);
        }

        public AnimationPlayer AnimationPlayer => animationPlayer;

        public ICreatureState CurrentState => currentState;

        public abstract long Id { get; }


        public override void _Process(double delta)
        {
            CurrentState.Process(delta);
        }

        protected void OnAnimationFinised(StringName name)
        {
            LOG.Debug("Animation finished.");
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
                if (child is ICreatureBodySprite creatureBody)
                {
                    return creatureBody.HoverRect();
                }
            }
            return EMPTY;
        }

 
        public void Remove()
        {
            QueueFree();
        }

        public Direction DirectionTo(ICreature another)
        {
            var anotherBody = ((AbstractCreature)another).GetNode<AbstractCreatureBodySprite>("Body");
            var thisBody = GetNode<AbstractCreatureBodySprite>("Body");
            return (thisBody.GlobalTextureCenter(thisBody.GlobalPosition) - anotherBody.GlobalTextureCenter(anotherBody.GlobalPosition)).GetDirection();
        }
    }
}