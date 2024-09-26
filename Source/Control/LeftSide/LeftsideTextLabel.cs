using Godot;

namespace y1000.Source.Control.LeftSide;

public partial class LeftsideTextLabel : RichTextLabel
{
    private double _time;

    public override void _Ready()
    {
        Clean();
    }
    public void Display(string text)
    {
        _time = 4;
        Text = "[color='yellow']" + text + "[/color]";
        Visible = true;
    }

    public double Time => _time;

    public void Clean()
    {
        _time = 0;
        Text = "";
        Visible = false;
    }

    public void Copy(LeftsideTextLabel another)
    {
        _time = another._time;
        Text = another.Text;
        Visible = another.Visible;
    }

    public void Update(double delta)
    {
        if (_time <= 0)
        {
            return;
        }
        _time -= delta;
        if (_time <= 0)
        {
            Clean();
        }
    }
}