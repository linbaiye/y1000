using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public partial class OffsetRect : TextureRect
{
    public override void _Process(double delta)
    {
        var texture = GetParent<Character>().BodyTexture;
        var parent = GetParent<Character>();
        Position = new (parent.Position.X + texture.Offset.X, parent.Position.Y);
    }

}