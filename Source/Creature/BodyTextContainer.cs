using Godot;
using NLog;
using y1000.code;
using y1000.Source.Player;

namespace y1000.Source.Creature;

public partial class BodyTextContainer : MarginContainer
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    public override void _Ready()
    {
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExit;
    }

    private void OnMouseEntered()
    {
        var bodySprite = GetParent<BodySprite>();
        var position = bodySprite.Coordinate + new Vector2(16, -16);
        GlobalPosition = position;
        LOGGER.Debug("Position {0}.", GlobalPosition);
        var label = GetNode<Label>("Label");
        label.Text = "牛";
        label.Show();
    }

    private void OnMouseExit()
    {
        var label = GetNode<Label>("Label");
        label.Hide();
    }
}