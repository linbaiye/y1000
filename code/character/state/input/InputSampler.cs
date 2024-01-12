using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.util;

namespace y1000.code.character.state.input
{
    public class InputSampler
    {

        private RightMousePressedMotion? PressedMotion { get; set; }

        public IInput? Sample(InputEvent inputEvent, Vector2 mousePosition)
        {
            if (inputEvent is InputEventMouseButton mouseButton)
            {
                if (mouseButton.ButtonIndex == MouseButton.Right)
                {
                    if (mouseButton.IsPressed())
                    {
                        Direction clickDirection = mousePosition.GetDirection();
                        return InputFactory.CreateMouseMoveInput(clickDirection);
                    }
                    else if (mouseButton.IsReleased())
                    {
                        return InputFactory.CreateMouseRightRelease();
                    }
                }
            }
            else if (inputEvent is InputEventMouseMotion mouseMotion)
            {
                if (mouseMotion.ButtonMask != MouseButtonMask.Right)
                {
                    return null;
                }
                var input = InputFactory.CreateRightMousePressedMotion(mousePosition.GetDirection());
                if (PressedMotion == null || PressedMotion.Direction != input.Direction)
                {
                    PressedMotion = input;
                }
                return input;
            }
            return null;
        }
    }
}