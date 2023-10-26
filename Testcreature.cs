using Godot;
using System;

public partial class Testcreature : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}


	public override void _Input(InputEvent @event)
	{
		var buffalo = GetNode<Buffalo>("Buffalo");
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.IsPressed())
			{
				switch (eventKey.Keycode)
				{
					case Key.Key1:
					buffalo.ChangeDirection(y1000.code.Direction.UP);
						break;
					case Key.Key2:
					buffalo.ChangeDirection(y1000.code.Direction.UP_RIGHT);
						break;
					case Key.Key3:
					buffalo.ChangeDirection(y1000.code.Direction.RIGHT);
						break;
					case Key.Up:
						buffalo.Move(y1000.code.Direction.UP);
						break;
					case Key.Right:
						buffalo.Move(y1000.code.Direction.RIGHT);
						break;
					case Key.Left:
						buffalo.Move(y1000.code.Direction.LEFT);
						break;
					case Key.Down:
						buffalo.Move(y1000.code.Direction.DOWN);
						break;
					case Key.A:
						buffalo.Attack();
						break;
				}
			}
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
