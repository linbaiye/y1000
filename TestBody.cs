using Godot;
using System;
using y1000.code;

public partial class TestBody : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 vector = VectorUtil.Velocity(Direction.RIGHT) * (float) delta;
		Position = Position + vector;
	}
}
