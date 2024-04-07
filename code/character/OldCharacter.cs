using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Godot;
using NLog;
using y1000.code.character.state;
using y1000.code.character.state.snapshot;
using y1000.code.creatures;
using y1000.code.entity.equipment.chest;
using y1000.code.networking;
using y1000.code.networking.message;
using y1000.code.player;
using y1000.code.player.state;
using y1000.code.util;
using y1000.code.world;
using y1000.Source.Input;

namespace y1000.code.character
{
    public partial class OldCharacter : Player
    {
        private WeaponType equippedWapon;

        private readonly StateSnapshotManager stateBuffers = new();

        private readonly InputSampler inputSampler = new InputSampler();

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public override void _Ready()
        {
            SetupAnimationPlayer();
            ChangeState(CharacterStateFactory.INSTANCE.CreateIdleState(this));
            Visible = false;
        }


        private void Rewind(UpdateMovmentStateMessage stateMessage)
        {
            IPlayerState state = stateMessage.Restore(this);
            AnimationPlayer.Stop();
            ChangeState(state);
        }


        public void HandleMessage(IEntityMessage message)
        {
            if (message is UpdateMovmentStateMessage stateMessage)
            {
                if (!stateBuffers.TryAck(stateMessage))
                {
                    stateBuffers.Reset(stateMessage.Sequence);
                    Rewind(stateMessage);
                }
            }
        }


        public void SendActAndSavePredict(IInput input, Action? afterSnapshot)
        {
            //SendMessage(input);
            afterSnapshot?.Invoke();
            var predicted = ((IOldCharacterState)CurrentState).Snapshot(this);
            stateBuffers.SaveState(input, predicted);
        }


        public bool CanMove(Point coordinate)
        {
            var parent = GetParent<Source.Game>();
            return parent != null && parent.CanMove(coordinate);
        }

        public void SendMessage(I2ServerGameMessage message)
        {
            //Console.Write("Test");
            LOG.Debug("Sending message " + message.ToString());
            var parent = GetParent<Source.Game>();
            parent.SendMessage(message);
        }

        public WeaponType EquippedWeapon 
        {
            get 
            {
                return equippedWapon;
            }
            set
            {
                equippedWapon = value;
            }
        }

        internal void MoveOrTurn(Direction clickDirection)
        {
            var next = Coordinate.Next(clickDirection);
            if (CanMove(next))
            {
                Move(clickDirection);
            }
            else
            {
                Turn(clickDirection);
            }
        }

        internal void MoveOrTurn(MouseRightClick click) 
        {
            var next = Coordinate.Next(click.Direction);
            if (CanMove(next))
            {

            }
            else
            {
                Turn(click.Direction);
            }
        }

        private void HandleDoubleClick(IOldCharacterState charState, InputEventMouseButton mouseButton, IEnumerable<ICreature> creatures)
        {
            var clickPoint = mouseButton.Position.ToVector2I();
            foreach (var creature in creatures)
            {
                if (creature.HoverRect().HasPoint(clickPoint))
                {
                    charState.OnMouseLeftDoubleClick(Input.IsPhysicalKeyPressed(Key.Ctrl), Input.IsPhysicalKeyPressed(Key.Shift), creature);
                    break;
                }
            }
        }

        private IOldCharacterState MyState => (IOldCharacterState)CurrentState;


        public void HandleInput(InputEvent @event)
        {
            var input = inputSampler.Sample(@event, GetLocalMousePosition());
            if (input == null)
            {
                return;
            }
            logger.Info("Sampled inputType " + input.Type);
            switch (input.Type) {
                case InputType.MOUSE_RIGHT_CLICK:
                    MyState.OnMouseRightClicked(this, (MouseRightClick)input);
                    break;
                case InputType.MOUSE_RIGHT_RELEASE:
                    MyState.OnMouseRightReleased(this, (MouseRightRelease)input);
                    break;
                case InputType.MOUSE_RIGHT_MOTION:
                    MyState.OnMouseMotion(this, (RightMousePressedMotion)input);
                    break;
            }
        }

        public void HandleMouseInput(GameMap gameMap, IEnumerable<ICreature> creatures, InputEventMouse inputEvent)
        {
            if (CurrentState is not IOldCharacterState charState)
            {
                return;
            }
            if (inputEvent is InputEventMouseButton mouseButton)
            {
                if (mouseButton.ButtonIndex == MouseButton.Right)
                {
                    if (mouseButton.IsPressed())
                    {
                        Direction clickDirection = GetLocalMousePosition().GetDirection();
                        var input = InputFactory.CreateMouseMoveInput(clickDirection);
                        charState.OnMouseRightClicked(this, input);
                    }
                    else if (mouseButton.IsReleased())
                    {
                        var input = InputFactory.CreateMouseRightRelease();
                        charState.OnMouseRightReleased(this, input);
                    }
                }
                else if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.DoubleClick)
                {
                    HandleDoubleClick(charState, mouseButton, creatures);
                }
            }
            else if (inputEvent is InputEventMouseMotion mouseMotion)
            {
                if (mouseMotion.ButtonMask == MouseButtonMask.Right)
                {
                    Direction clickDirection = GetLocalMousePosition().GetDirection();
                    charState.OnMouseMotion(clickDirection);
                }
            }
        }
    }
}