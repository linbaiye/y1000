using Godot;

namespace y1000.Source.Control;

public partial class LeftupText : TextureRect
{

    private Label _label;
    private double _time;

    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
    }

    public void Display(string text)
    {
        Visible = true;
        _label.Text = text;
        _time = 4;
    }

    public override void _Process(double delta)
    {
        if (_time <= 0)
        {
            return;
        }
        _time -= delta;
        if (_time <= 0)
        {
            Visible = false;
        }
    }
}