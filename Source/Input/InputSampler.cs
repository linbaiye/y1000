using Godot;
using NLog;
using y1000.code;

namespace y1000.Source.Input
{
    public class InputSampler
    {

        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

        private IInput? SampleMouseButton(InputEventMouseButton button, Vector2 mouseOffset)
        {
            if (button.ButtonIndex == MouseButton.Right)
            {
                if (button.IsPressed())
                {
                    return InputFactory.CreateMouseMoveInput(mouseOffset.GetDirection());
                }

                if (button.IsReleased())
                {
                    return InputFactory.CreateMouseRightRelease();
                }
            }
            else if (button.ButtonIndex == MouseButton.Left)
            {
                if (button.IsPressed() && button.DoubleClick)
                {
                    if ((button.GetModifiersMask() & KeyModifierMask.MaskShift) != 0)
                    {
                        LOGGER.Debug("Masked shift.");
                    }
                    LOGGER.Debug("Clicked at coordinate {0}, position {1}.", mouseOffset.ToCoordinate(), mouseOffset);
                }
            }
            return null;
        }

        private IInput? SampleMouseMotion(InputEventMouseMotion mouseMotion, Vector2 mouseOffset)
        {
            return mouseMotion.ButtonMask != MouseButtonMask.Right ? null : 
                InputFactory.CreateRightMousePressedMotion(mouseOffset.GetDirection());
        }

        private IInput? SampleKeyEvent(InputEventKey eventKey)
        {
            return InputFactory.KeyInput(eventKey.Keycode);
        }
        
        public IInput? Sample(InputEvent inputEvent, Vector2 mouseOffset)
        {
            return inputEvent switch
            {
                InputEventMouseButton mouseButton => SampleMouseButton(mouseButton, mouseOffset),
                InputEventMouseMotion mouseMotion => SampleMouseMotion(mouseMotion, mouseOffset),
                InputEventKey eventKey => SampleKeyEvent(eventKey),
                _ => null
            };
        }
    }
}