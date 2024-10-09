using System;
using Godot;

namespace y1000.Source.Control;

public partial class DialogPopup : PanelContainer
{
    private const int MaxWidth = 140;

    private Label _label;

    private Timer _timer;
    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
        _timer = GetNode<Timer>("Timer");
        _label.Resized += OnResized;
        _timer.Timeout += OnTimeout;
        Visible = false;
    }

    private void OnResized()
    {
        CustomMinimumSize = new Vector2(Math.Min(MaxWidth, Size.X), Size.Y);
        if (Size.X > MaxWidth)
        {
            _label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _label.HorizontalAlignment = HorizontalAlignment.Left;
            _label.VerticalAlignment = VerticalAlignment.Top;
        }
    }

    private void OnTimeout()
    {
        Visible = false;
    }
    
    public void Display(string text)
    {
        ZIndex = 2;
        _label.Text = text;
        Visible = true;
        _timer.Start();
    }

    public void Delete()
    {
        Visible = false;
        _timer.Stop();
        QueueFree();
    }
}