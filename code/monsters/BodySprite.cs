using Godot;
using System;
using y1000.code.creatures;

public partial class BodySprite : Sprite2D
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var parent = GetParent<AbstractCreature>();
		var texture = parent.BodyTexture;
		Position = texture.Position;
		Texture = texture.Texture;
	}
}
