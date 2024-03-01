using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code;
using y1000.code.character;
using y1000.code.character.state;
using y1000.code.character.state.input;
using y1000.code.character.state.Prediction;
using y1000.code.networking.message;
using y1000.code.networking.message.character;
using y1000.code.player;
using y1000.Source.Character.State;
using CharacterIdleState = y1000.Source.Character.State.CharacterIdleState;

namespace y1000.Source.Character
{
    public partial class Character : Node2D, IBody
    {
        //private Character? _character;
        private ICharacterState _state;

        private Direction _direction;

        private readonly InputSampler inputSampler;

        private readonly PredictionManager _predictionManager;

        private Character()
        {
            inputSampler = new InputSampler();
            _state = EmptyState.Instance;
            _predictionManager = new PredictionManager();
        }

        public Direction Direction => _direction;

        public void ChangeState(ICharacterState state)
        {
            _state = state;
        }

        public override void _Ready()
        {
            base._Ready();
        }

        public Vector2I Coordinate => Position.ToCoordinate();

        public override void _Process(double delta)
        {
            base._Process(delta);
            _state.Process(this, delta);
        }

        private void HandleInput<T>(Func<Character, T, IPrediction> predictionFunc, Action<Character, T> handler, T input) where T : IInput
        {
            var prediction = predictionFunc.Invoke(this, input);
            handler.Invoke(this, input);
            _predictionManager.Save(prediction);
        }

        public void HandleInputEvent(InputEvent @event)
        {
            var input = inputSampler.Sample(@event, GetLocalMousePosition());
            if (input == null)
            {
                return;
            }
            switch (input.Type) {
                case InputType.MOUSE_RIGH_CLICK:
                    HandleInput(_state.Predict, _state.OnMouseRightClicked, (MouseRightClick)input);
                    break;
            }
        }

        public bool IsMale => true;

        public void HandleMessage(ICharacterMessage characterMessage)
        {
            bool ret = _predictionManager.Reconcile(characterMessage);
            if (!ret)
            {
            }
        }

        public OffsetTexture BodyOffsetTexture => _state.BodyOffsetTexture(this);


        public static Character LogedIn(LoginMessage message)
        {
            PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/character.tscn");
            var character = scene.Instantiate<Character>();
            character._state = new CharacterIdleState();
            character.Position = message.Coordinate.ToPosition();
            character._direction = Direction.DOWN;
            character.ZIndex = 1;
            character.Visible = true;
            return character;
        }
    }
}