using Godot;
using y1000.code.creatures;

namespace y1000.code.player;

public partial class OffsetRect : TextureRect
{
    public override void _Process(double delta)
    {
        var texture = GetParent<AbstractCreature>().BodyTexture;
        var parent = GetParent<AbstractCreature>();
        Position = new (parent.Position.X + texture.Offset.X, parent.Position.Y);
    }

}