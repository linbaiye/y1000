using Godot;

namespace y1000.Source.Creature;

public partial class LifePercentBar : TextureProgressBar
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

    public void Display(int percent)
    {
        _time = 2;
        Value = percent;
        Visible = true;
    }
}