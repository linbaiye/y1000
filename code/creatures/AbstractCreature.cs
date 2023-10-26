using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.player;

namespace y1000.code.creatures
{
    public abstract partial class AbstractCreature : StaticBody2D, ICreature
    {
        private ICreatureState currentState = UnknownState.INSTANCE;

        internal void ChangeState(ICreatureState newState)
        {
            currentState = newState;
        }

        public State State => currentState.State;

        public Direction Direction => currentState.Direction;

        public abstract PositionedTexture BodyTexture { get; }

        public void Move(Direction direction)
        {
            currentState.Move(direction);
        }

        public ICreatureState CurrentState => currentState;
    }
}