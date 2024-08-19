using Godot;

namespace y1000.Source.Control.LeftSide;

public partial class LeftsideTextLabel : RichTextLabel
{
    private double _time;
    public void SetText(string text)
    {
        _time = 4;
        Text = "[color='yellow']" + text + "[/color]";
        Visible = true;
    }

    public double Time => _time;

    public void SetText(string text, double time)
    {
        _time = time;
        Text = "[color='yellow']" + text + "[/color]";
        Visible = true;
    }
    
    public bool Update(double delta)
    {
        if (_time <= 0)
        {
            return true;
        }
        _time -= delta;
        if (_time <= 0)
        {
            Text = "";
            Visible = false;
            return true;
        }
        return false;
    }
}