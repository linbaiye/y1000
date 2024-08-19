using Godot;

namespace y1000.Source.Control.Bottom;

public partial class InputEdit : LineEdit
{
    private int _counter;
    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event is not InputEventKey inputEvent || inputEvent.Keycode != Key.Enter)
            return;
        if (HasFocus() && inputEvent.IsReleased())
        {
            if (--_counter == 0)
            {
                return;
            }
            Text = "";
            ReleaseFocus();
        }
        else
        {
            if (!HasFocus() && inputEvent.Pressed)
            {
                GrabFocus();
                _counter = 1;
            }
        }
        AcceptEvent();
    }
}
