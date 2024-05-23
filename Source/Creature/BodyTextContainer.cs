using Godot;
using NLog;
using y1000.code;
using y1000.Source.Animation;
using y1000.Source.Player;

namespace y1000.Source.Creature;

public partial class BodyTextContainer : Panel
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    public override void _Ready()
    {
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExit;
    }

    private void OnMouseEntered()
    {
        //LOGGER.Debug("Mouse Entered.");
        // var bodySprite = GetParent<Animation.BodySprite>();
        // var position = bodySprite.OwnerPosition + new Vector2(16, -16);
        // GlobalPosition = position;
         var bodySprite = GetParent<BodySprite>();
         Position = bodySprite.Offset;
        var label = GetNode<Label>("Label");
        label.GlobalPosition = bodySprite.BodyPosition;
         label.Text = "牛";
         label.Show();
    }

    public override void _Process(double delta)
    {
         var bodySprite = GetParent<BodySprite>();
         Position = bodySprite.Offset;
    }

    private void OnMouseExit()
    {
        LOGGER.Debug("Mouse Exited.");
        var label = GetNode<Label>("Label");
        label.Hide();
    }
}