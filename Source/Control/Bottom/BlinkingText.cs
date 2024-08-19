using Godot;

namespace y1000.Source.Control.Bottom;

public partial class BlinkingText : RichTextLabel
{
    private double _blinkingTime;
    
    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Disabled;
    }

    public void Blink(string text)
    {
        if (!string.IsNullOrEmpty(text) && _blinkingTime <= 0)
        {
            Text = "[center][pulse freq=8 color=#6193df ease=-2.0]" + text + "[/pulse][/center]";
            ProcessMode = ProcessModeEnum.Inherit;
            _blinkingTime = 1.2;
        }
    }

    public override void _Process(double delta)
    {
        _blinkingTime -= delta;
        if (_blinkingTime <= 0)
        {
            Text = "";
            ProcessMode = ProcessModeEnum.Disabled;
        }
    }
}