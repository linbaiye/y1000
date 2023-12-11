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
        private long lastSampleTime = 0;

        private InputEventMouseMotion? PreviousMotion { get; set; }

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
                var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                if (PreviousMotion == null)
                {
                    PreviousMotion = mouseMotion;
                    lastSampleTime = now;
                }
                else if (now > lastSampleTime + 30)
                {
                    lastSampleTime = now;
                    return InputFactory.CreateMouseMoveInput(mousePosition.GetDirection());
                }
            }
            return null;
        }
    }
}