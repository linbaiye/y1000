using Godot;

namespace y1000.Source.Player;

public partial class KungFuTip : Label
{
    private double _time;

    public override void _Ready()
    {
        Visible = false;
    }

    public override void _Process(double delta)
    {
        _time -= delta;
        if (_time <= 0)
        {
            Visible = false;
        }
    }

    public void Display(string text)
    {
        _time = 2;
        Text = text;
        Visible = true;
    }
}