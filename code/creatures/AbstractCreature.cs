using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures.state;
using y1000.code.player;

namespace y1000.code.creatures
{
    public abstract partial class AbstractCreature : StaticBody2D, ICreature
    {
        public float gravity = 0;

        private ICreatureState currentState = UnknownState.INSTANCE;

        private static readonly AnimationPlayer NULL_PLAYER = new();

        private AnimationPlayer animationPlayer;

	    private SpriteContainer spriteContainer = SpriteContainer.EmptyContainer;

        protected AbstractCreature()
        {
            animationPlayer = NULL_PLAYER;
        }

        protected void Setup(string spriteName)
        {
            // Customing AnimaationPlayer might be a better idea.
            animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            SetMeta("spriteNumber", 0);
            spriteContainer= SpriteContainer.LoadSprites(spriteName);
		    Position = Position.Snapped(VectorUtil.TILE_SIZE);
            animationPlayer.AnimationFinished += OnAnimationFinised;
        }

        internal void ChangeState(ICreatureState newState)
        {
            currentState = newState;
            currentState.PlayAnimation();
        }

        public State State => currentState.State;

        public Direction Direction => currentState.Direction;

        public PositionedTexture BodyTexture
        {
            get
            {
                int nr = (int)GetMeta("spriteNumber");
                return spriteContainer.Get(currentState.GetSpriteOffset() + nr);
            }
        }

        public void Move(Direction direction)
        {
            currentState.Move(direction);
        }

        public AnimationPlayer AnimationPlayer => animationPlayer;

        public ICreatureState CurrentState => currentState;

        public override void _Process(double delta)
        {
            if (CurrentState is AbstractCreatureMoveState moveState)
            {
                moveState.PhysicsProcess(delta);
            }
        }


        protected void OnAnimationFinised(StringName name)
        {
            CurrentState.OnAnimationFinised();
        }

        public void ChangeDirection(Direction newDirection)
        {
            CurrentState.ChangeDirection(newDirection);
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

        public Rectangle Rectangle()
        {
            throw new NotImplementedException();
        }

        public long Id()
        {
            throw new NotImplementedException();
        }

    }
}