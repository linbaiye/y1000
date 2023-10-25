using Godot;
using System;

public partial class GameHud : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Slot slot = GetNode<Slot>("RightSide/GridContainer/Panel");
		slot.LoadItem("000003");
		slot = GetNode<Slot>("RightSide/GridContainer/Panel3");
		slot.LoadItem("000010");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
