using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.character.state.input
{
    public class InputSampler
    {
        private long lastSampleTime = 0;

        private InputEvent? InputEvent {get; set;}

        public IInput? GetInput()
        {
            if (InputEvent == null) {
                return null;
            }
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            if (now - lastSampleTime >= 20)
            {
            }
            return null;
        }
    }
}

/*
        private IInput ParseInputEvent()
        {
            if (InputEvent is InputEventMouseButton mouseButton)
            {
                if (mouseButton.ButtonIndex == MouseButton.Right)
                {
                    if (mouseButton.IsPressed())
                    {
                        Direction clickDirection = GetLocalMousePosition().GetDirection();
                        var input = InputFactory.CreateMouseMoveInput(clickDirection);
                        charState.OnMouseRightClick(this, input);
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

        public void Sample(InputEvent inputEvent)
        {
            if (inputEvent is InputEventMouseButton mouseButton)
            {
                if (mouseButton.ButtonIndex == MouseButton.Right)
                {
                    if (mouseButton.IsPressed())
                    {
                        Direction clickDirection = GetLocalMousePosition().GetDirection();
                        var input = InputFactory.CreateMouseMoveInput(clickDirection);
                        charState.OnMouseRightClick(this, input);
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
}*/