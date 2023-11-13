using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;

public partial class OffsetRect : TextureRect
{
    public override void _Process(double delta)
    {
        var texture = GetParent<AbstractCreature>().BodyTexture;
        var parent = GetParent<AbstractCreature>();
        Position = new (parent.Position.X + texture.Offset.X, parent.Position.Y);
    }

}