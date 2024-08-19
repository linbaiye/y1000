using Godot;

namespace y1000.Source.Control.Bottom;

public partial class UsedKungFuLabel : RichTextLabel
{
    private string _kungFuName = "";

    private double _blinkingTime;
    
    public void SetKungFuName(string text)
    {
        _kungFuName = text;
        _blinkingTime = 0;
        Text = CenterText();
        ProcessMode = ProcessModeEnum.Disabled;
    }

    public void Reset()
    {
        _kungFuName = "";
        Text = "";
    }

    public bool BlinkIfMatches(string text)
    {
        if (_kungFuName.Equals(text))
        {
            ProcessMode = ProcessModeEnum.Inherit;
            _blinkingTime = 1.2;
            Text = "[pulse freq=8 color=#6193df ease=-2.0]" + CenterText() + "[/pulse]";
            //Text = "[rainbow freq=2.0 sat=0.8 val=1]" + CenterText() + "[/rainbow]";
            return true;
        }
        return false;
    }

    public override void _Process(double delta)
    {
        _blinkingTime -= delta;
        if (_blinkingTime <= 0)
        {
            Text = CenterText();
            ProcessMode = ProcessModeEnum.Disabled;
        }
    }

    private string CenterText()
    {
        return "[center]" + _kungFuName + "[/center]";
    }

}