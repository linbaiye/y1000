using Godot;
using NLog;
using y1000.Source.Player;

namespace y1000.Source.Creature;

public partial class BodyTextContainer : CenterContainer
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    public override void _Ready()
    {
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExit;
    }

    private void AlignPosition()
    {
        var bodySprite = GetParent<BodySprite>();
        Position = bodySprite.Offset;
        var size = bodySprite.Texture.GetSize();
        Size = size;
    }

    private void OnMouseEntered()
    {
        AlignPosition();
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