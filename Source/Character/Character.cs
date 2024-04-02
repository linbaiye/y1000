using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Godot;
using NLog.Fluent;
using y1000.code;
using y1000.code.character;
using y1000.code.character.state;
using y1000.code.character.state.input;
using y1000.code.character.state.Prediction;
using y1000.code.networking.message;
using y1000.code.networking.message.character;
using y1000.code.player;
using y1000.code.util;
using y1000.code.world;
using y1000.Source.Character.State;
using CharacterIdleState = y1000.Source.Character.State.CharacterIdleState;

namespace y1000.Source.Character
{
    public partial class Character : Node2D, IBody
    {
        private ICharacterState _state;

        public Direction Direction { get; set; }

        public IRealm Realm { get; set; }

        private Character()
        {
            _state = EmptyState.Instance;
            Realm = IRealm.Empty;
        }

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
            _state.Process(this, (long)(delta * 1000));
        }

        public bool CanHandle(IInput input)
        {
            return _state.CanHandle(input);
        }

        public IPrediction Predict(IInput input)
        {
            switch (input.Type)
            {
                case InputType.MOUSE_RIGH_CLICK:
                    return _state.Predict(this, (MouseRightClick)input);
                case InputType.MOUSE_RIGHT_RELEASE:
                    return _state.Predict(this, (MouseRightRelease)input);
                case InputType.MOUSE_RIGHT_MOTION:
                    return _state.Predict(this, (RightMousePressedMotion)input);
                default:
                    throw new NotSupportedException();
            }
        }

        public bool CanMoveOneUnit(Direction direction)
        {
            return Realm.CanMove(Coordinate.Move(direction));
        }

        public void HandleInput(IInput input)
        {
            if (!_state.CanHandle(input))
            {
                return;
            }
            switch (input.Type)
            {
                case InputType.MOUSE_RIGH_CLICK:
                    _state.OnMouseRightClicked(this, (MouseRightClick)input);
                    break;
                case InputType.MOUSE_RIGHT_RELEASE:
                    _state.OnMouseRightReleased(this, (MouseRightRelease)input);
                    break;
                case InputType.MOUSE_RIGHT_MOTION:
                    _state.OnMousePressedMotion(this, (RightMousePressedMotion)input);
                    break;
            }
        }

        public bool IsMale => true;


        public OffsetTexture BodyOffsetTexture => _state.BodyOffsetTexture(this);

        public static Character LogedIn(LoginMessage message, IRealm realm)
        {
            PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/character.tscn");
            var character = scene.Instantiate<Character>();
            character._state = CharacterIdleState.ForMale();
            character.Position = message.Coordinate.ToPosition();
            character.Direction = Direction.DOWN;
            character.ZIndex = 3;
            character.Visible = true;
            character.Realm = realm;
            return character;
        }
    }
}