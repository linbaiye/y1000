using Godot;
using NLog;
using y1000.code;

namespace y1000.Source.Input
{
    public class InputSampler
    {

        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

        private IPredictableInput? SampleMouseButton(InputEventMouseButton button, Vector2 mouseOffset)
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
            return null;
        }

        private IPredictableInput? SampleMouseMotion(InputEventMouseMotion mouseMotion, Vector2 mouseOffset)
        {
            return mouseMotion.ButtonMask != MouseButtonMask.Right ? null : 
                InputFactory.CreateRightMousePressedMotion(mouseOffset.GetDirection());
        }

        private IPredictableInput SampleKeyEvent(InputEventKey eventKey)
        {
            return InputFactory.KeyInput(eventKey.Keycode);
        }
        
        public IPredictableInput? SampleMoveInput(InputEvent inputEvent, Vector2 mouseOffset)
        {
            return inputEvent switch
            {
                InputEventMouseButton mouseButton => SampleMouseButton(mouseButton, mouseOffset),
                InputEventMouseMotion mouseMotion => SampleMouseMotion(mouseMotion, mouseOffset),
                InputEventKey eventKey => SampleKeyEvent(eventKey),
                _ => null
            };
        }

        public IInput? SampleLeftClickInput(InputEventMouseButton button, long targetId)
        {
            if (button.DoubleClick)
            {
                if ((button.GetModifiersMask() & KeyModifierMask.MaskCtrl) != 0)
                {
                    LOGGER.Debug("Player attack");
                }
                else if ((button.GetModifiersMask() & KeyModifierMask.MaskShift) != 0)
                {
                    LOGGER.Debug("Monster attack");
                }
                return null;
            }
            else
            {
                return new LeftClick(targetId);
            }
        }
    }
}