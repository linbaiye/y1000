using Godot;
using System;

public partial class BodySprite : Sprite2D
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var parent = GetParent<Buffalo>();
		var texture = parent.BodyTexture;
		Position = texture.Position;
		Texture = texture.Texture;
	}
}
