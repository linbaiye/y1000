using Godot;
using NLog;
using y1000.Source.Entity;
using y1000.Source.Util;

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

        private IPredictableInput? SampleKeyEvent(InputEventKey eventKey)
        {
            if (eventKey.Keycode >= Key.F1 && eventKey.Keycode <= Key.F12 && eventKey.IsPressed())
            {
                return InputFactory.KeyInput(eventKey.Keycode);
            }

            return null;
        }
        
        public IPredictableInput? SampleInput(InputEvent inputEvent, Vector2 mouseOffset)
        {
            return inputEvent switch
            {
                InputEventMouseButton mouseButton => SampleMouseButton(mouseButton, mouseOffset),
                InputEventMouseMotion mouseMotion => SampleMouseMotion(mouseMotion, mouseOffset),
                InputEventKey eventKey => SampleKeyEvent(eventKey),
                _ => null
            };
        }

        public RightMousePressedMotion SampleMoveInput(Vector2 mouseOffset)
        {
            return InputFactory.CreateRightMousePressedMotion(mouseOffset.GetDirection());
        }

        public IInput? SampleEntityClickInput(InputEventMouseButton button, IEntity entity, Vector2 mouseOffset)
        {
            if (button.ButtonIndex == MouseButton.Left)
            {
                return SampleLeftClickInput(button, entity);
            }
            else
            {
                return SampleMouseButton(button, mouseOffset);
            }
        }

        private IInput? SampleLeftClickInput(InputEventMouseButton button, IEntity entity)
        {
            if (button.DoubleClick)
            {
                if ((button.GetModifiersMask() & KeyModifierMask.MaskShift) != 0)
                {
                    return InputFactory.CreateAttack(entity);
                }
            }
            else if (button.IsPressed() && button.GetModifiersMask() == 0)
            {
                return new CreatureLeftClick(entity);
            }
            return null;
        }
    }
}