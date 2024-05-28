using System;
using Godot;
using NLog;

namespace y1000.Source.Control.Bottom;

public partial class AvatarPart : TextureRect
{
    private static readonly ILogger LOG = LogManager.GetCurrentClassLogger();
    public override void _Ready()
    {
        GuiInput += OnInput;
    }

    private void OnInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton { DoubleClick: true } button &&
            (button.ButtonMask & MouseButtonMask.Left) != 0 && Texture != null)
        {
            GetParent<Avatar>().OnPartClicked(this, Name);
        }
    }
}