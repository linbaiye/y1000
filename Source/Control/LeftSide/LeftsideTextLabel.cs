using System.Net.Mime;
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
        _time = 1;
        Text = "[color='yellow']" + text + "[/color]";
        Visible = true;
    }

    public double Time => _time;

    public void Clean()
    {
        _time = 0;
        Text = "";
        //Visible = false;
    }

    public void Copy(LeftsideTextLabel another)
    {
        Text = another.Text;
        //Visible = another.Visible;
        _time = 1;
    }

    public bool Timeout => _time <= 0 && !string.IsNullOrEmpty(Text);

    public bool Empty => string.IsNullOrEmpty(Text);

    public bool Available => _time <= 0 && string.IsNullOrEmpty(Text);

    public void Update(double delta)
    {
        if (_time > 0)
        {
            _time -= delta;
        }
    }
}