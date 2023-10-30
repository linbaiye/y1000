using Godot;
using System;
using y1000.code;

public partial class TestBody : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Velocity = VectorUtil.Velocity(Direction.RIGHT) * (float) delta;
		MoveAndCollide(Velocity);
	}
}
