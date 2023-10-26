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
		GD.Print("ssss");
		GetNode<Label>("Label").Show();
	}


	public void InputEvent(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton button && button.DoubleClick && button.ButtonIndex == MouseButton.Left)
		{
			GD.Print("double clicked");
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
