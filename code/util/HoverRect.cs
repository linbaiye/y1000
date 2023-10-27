using Godot;
using System;
using y1000.code.creatures;

public partial class HoverRect: TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RectMouseEntered()
	{
		var parent = GetParent<Sprite2D>();
		GD.Print("test");
		if (parent != null)
		{
			//int width = (parent.Texture.GetWidth() - 20) / 2;
			//int height = parent.Texture.GetHeight() / 2;
			Size = new Vector2(parent.Texture.GetWidth() - 20, parent.Texture.GetHeight());
			//var p1 = parent.GetParent<StaticBody2D>();
			//Position = new Vector2(width, height);
		}
		GetNode<Label>("Label").Show();
	}


	public void InputEvent(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton button && button.DoubleClick && button.ButtonIndex == MouseButton.Left)
		{
			if (GetParent() is AbstractCreature creature)
			{
				GD.Print("clicked creature");
			}
		}
	}

	public void RectMouseExited()
	{
		GetNode<Label>("Label").Hide();
	}
}
