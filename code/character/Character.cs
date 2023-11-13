using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using y1000.code.character.state;
using y1000.code.creatures;
using y1000.code.player;
using y1000.code.util;
using y1000.code.world;

namespace y1000.code.character
{
    public partial class Character : Player
    {
        private Weapon equippedWapon;
        public override void _Ready()
        {
            Setup();
            ChangeState(CharacterStateFactory.INSTANCE.CreateIdleState(this));
        }


        private void HandleLeftButtonInput(IEnumerable<ICreature> creatures, InputEventMouseButton input)
        {
            Direction direction = GetLocalMousePosition().MouseDirection();
        }


        public bool CanMove(Point coordinate)
        {
            var parent = GetParent<Game>();
            return parent != null && parent.CanMove(coordinate);
        }

        public Weapon EquippedWeapon 
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


        private void HandleRightButtonInput(GameMap map, IEnumerable<ICreature> creatures, InputEventMouseButton input)
        {
        }

        private void HanldeMouseMotion(GameMap gameMap, IEnumerable<ICreature> creatures, InputEventMouseMotion mouseMotion)
        {
        }

        private void HandleDoubleClick(ICharacterState charState, InputEventMouseButton mouseButton, IEnumerable<ICreature> creatures)
        {
            var clickPoint = mouseButton.Position.ToVector2I();
            foreach (var creature in creatures)
            {
                if (creature.HoverRect().HasPoint(clickPoint))
                {
                    GD.Print("Double clicked at " + mouseButton.Position);
                    charState.OnMouseLeftDoubleClick(Input.IsPhysicalKeyPressed(Key.Ctrl), Input.IsPhysicalKeyPressed(Key.Shift), creature);
                    break;
                }
            }
        }

        protected static Direction ComputeDirection(Vector2 mousePosition)
        {
            var angle = Mathf.Snapped(mousePosition.Angle(), Mathf.Pi / 4) / (Mathf.Pi / 4);
            int dir = Mathf.Wrap((int)angle, 0, 8);
            return dir switch
            {
                0 => Direction.RIGHT,
                1 => Direction.DOWN_RIGHT,
                2 => Direction.DOWN,
                3 => Direction.DOWN_LEFT,
                4 => Direction.LEFT,
                5 => Direction.UP_LEFT,
                6 => Direction.UP,
                7 => Direction.UP_RIGHT,
                _ => throw new NotSupportedException(),
            };
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
                        Direction clickDirection = ComputeDirection(GetLocalMousePosition());
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
                    Direction clickDirection = ComputeDirection(GetLocalMousePosition());
                    charState.OnMouseMotion(clickDirection);
                }
            }
        }
    }
}