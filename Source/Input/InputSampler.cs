using Godot;
using y1000.code;

namespace y1000.Source.Input
{
    public class InputSampler
    {

        private RightMousePressedMotion? PressedMotion { get; set; }

        public IInput? Sample(InputEvent inputEvent, Vector2 mousePosition)
        {
            switch (inputEvent)
            {
                case InputEventMouseButton mouseButton:
                {
                    if (mouseButton.ButtonIndex == MouseButton.Right)
                    {
                        if (mouseButton.IsPressed())
                        {
                            return InputFactory.CreateMouseMoveInput(mousePosition.GetDirection());
                        }
                        if (mouseButton.IsReleased())
                        {
                            return InputFactory.CreateMouseRightRelease();
                        }
                    }
                    break;
                }
                case InputEventMouseMotion mouseMotion when mouseMotion.ButtonMask != MouseButtonMask.Right:
                    return null;
                case InputEventMouseMotion:
                {
                    var input = InputFactory.CreateRightMousePressedMotion(mousePosition.GetDirection());
                    if (PressedMotion == null || PressedMotion.Direction != input.Direction)
                    {
                        PressedMotion = input;
                    }
                    return input;
                }
            }
            return null;
        }
    }
}