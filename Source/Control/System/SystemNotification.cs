using Godot;

namespace y1000.Source.Control.System;

public partial class SystemNotification : NinePatchRect
{

    private Label _text;
    
    private Button _close;
    
    private TextureButton _confirm;

    public override void _Ready()
    {
        _confirm = GetNode<TextureButton>("Confirm");
        _close = GetNode<Button>("Close");
        _text = GetNode<Label>("Text");
        _close.Pressed += () => Visible = false;
        _confirm.Pressed += () => Visible = false;
    }

    public void Display(string text)
    {
        _text.Text = text;
        Visible = true;
    }
}