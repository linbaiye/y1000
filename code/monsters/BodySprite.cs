using Godot;
using System;
using y1000.code.creatures;

public partial class BodySprite : Sprite2D
{
	public override void _Process(double delta)
	{
		var parent = GetParent<AbstractCreature>();
		var texture = parent.BodyTexture;
		Offset = texture.Position;
		Texture = texture.Texture;
	}
}
