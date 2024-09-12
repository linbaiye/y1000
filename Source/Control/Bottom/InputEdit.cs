using System.Net.Mime;
using Godot;
using NLog;
using y1000.Source.Character;

namespace y1000.Source.Control.Bottom;

public partial class InputEdit : LineEdit
{
    private int _counter;
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private CharacterImpl? _character;

    public void BindCharacter(CharacterImpl character)
    {
        _character = character;
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event is not InputEventKey inputEvent) 
            return;
        if (inputEvent.Keycode == Key.Backspace && inputEvent.IsPressed() && HasFocus())
        {
            if (!string.IsNullOrEmpty(Text) && Text.Length > 1)
            {
                Text = Text.Substring(0, Text.Length - 1);
            }
            AcceptEvent();
            return;
        }
        if (inputEvent.Keycode != Key.Enter)
            return;
        if (HasFocus() && inputEvent.IsReleased())
        {
            if (--_counter == 0)
            {
                return;
            }
            if (!string.IsNullOrEmpty(Text))
                _character?.Chat(Text);
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
