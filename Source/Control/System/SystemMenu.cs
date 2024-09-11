using System;
using Godot;

namespace y1000.Source.Control.System;

public partial class SystemMenu : NinePatchRect
{

    private TextureButton _setting;
    private TextureButton _return;
    private TextureButton _exit;

    public event Action? SettingPressed;
    public override void _Ready()
    {
        _setting = GetNode<TextureButton>("SettingButton");
        _return = GetNode<TextureButton>("ReturnButton");
        _exit = GetNode<TextureButton>("ExitButton");
        _return.Pressed += () => Visible = false;
        _setting.Pressed += OnSettingPressed;
        Visible = false;
    }

    private void OnSettingPressed()
    {
        SettingPressed?.Invoke();
    }

    public void OnSystemButtonClicked()
    {
        Visible = !Visible;
    }
}