using Godot;
using NLog;

namespace y1000.Source.Control.Bottom;

public partial class AvatarPart : TextureRect
{
    private static readonly ILogger LOG = LogManager.GetCurrentClassLogger();
    public string Text { get; set; } = "";
    public override void _Ready()
    {
        GuiInput += OnInput;
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    private void OnInput(InputEvent inputEvent)
    {
        if (inputEvent is not InputEventMouseButton button)
        {
            return;
        }
        if (button.DoubleClick && (button.ButtonMask & MouseButtonMask.Left) != 0 && Texture != null)
        {
            GetParent<Avatar>().OnPartClicked(this, Name);
        }
        else if (button.Pressed && (button.ButtonMask & MouseButtonMask.Right) != 0)
        {
            
        }
    }

    private void OnMouseEntered()
    {
        GetParent<Avatar>().OnMouseEntered(this);
    }
    
    private void OnMouseExited()
    {
        GetParent<Avatar>().OnMouseExited(this);
    }
}