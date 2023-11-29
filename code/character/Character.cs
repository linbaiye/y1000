using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Godot;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using y1000.code.character.state;
using y1000.code.creatures;
using y1000.code.entity.equipment.chest;
using y1000.code.networking.message;
using y1000.code.player;
using y1000.code.util;
using y1000.code.world;

namespace y1000.code.character
{
    public partial class Character : Player
    {
        private WeaponType equippedWapon;


        public override void _Ready()
        {
            Setup();
            ChangeState(CharacterStateFactory.INSTANCE.CreateIdleState(this));
        }


        public void HandleMessage(IGameMessage message)
        {
            if (message is PositionMessage) {
                
            }
        }



        public bool CanMove(Point coordinate)
        {
            var parent = GetParent<Game>();
            return parent != null && parent.CanMove(coordinate);
        }

        public void SendMessage(IGameMessage message)
        {
            var parent = GetParent<Game>();
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

        private void HandleDoubleClick(ICharacterState charState, InputEventMouseButton mouseButton, IEnumerable<ICreature> creatures)
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


        public void HandleMouseInput(GameMap gameMap, IEnumerable<ICreature> creatures, InputEventMouse inputEvent)
        {
            if (CurrentState is not ICharacterState charState)
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
                        charState.OnMouseRightClick(clickDirection);
                    }
                    else if (mouseButton.IsReleased())
                    {
                        charState.OnMouseRightReleased();
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