using Godot;
using System;
using y1000.code;
using y1000.code.creatures;
using y1000.code.util;

public partial class Game : Node2D
{
	// Called when the node enters the scene tree for the first time.

	private Character? character;



	public override void _Ready()
	{
		character = GetNode<Character>("Character");
	}


	private void HandleMouseInput(InputEventMouse eventMouse)
	{
		if (eventMouse is InputEventMouseButton button)
		{
			if (button.ButtonIndex == MouseButton.Right)
			{
				if (button.IsPressed())
				{
					character?.Move(GetLocalMousePosition());
				} else if (button.IsReleased())
				{
					character?.StopMove();
				}
			}
			else if (button.ButtonIndex == MouseButton.Left)
			{
				if (button.IsPressed() && button.DoubleClick)
				{
					var children = GetChildren();
					foreach (var child in children)
					{
						if (child is AbstractCreature creature)
						{
							if (creature.CollisionRect().HasVector(GetGlobalMousePosition()) )
							{
								if (Input.IsPhysicalKeyPressed(Key.Shift))
								{
									creature.Hurt();
								}
							}
						}
					}
				}
			}
		}
		else if (eventMouse is InputEventMouseMotion mouseMotion)
		{
			if ((mouseMotion.ButtonMask & MouseButtonMask.Right) != 0)
			{
				character?.Move(GetLocalMousePosition());
			}
		}
	}

	private void HandleKeyInput(InputEventKey eventKey)
	{
		var monster = GetNode<Buffalo>("Monster");
		if (eventKey.IsPressed())
		{
			switch (eventKey.Keycode)
			{
				case Key.Left:
					monster.Move(Direction.LEFT);
					break;
				case Key.Right:
					monster.Move(Direction.RIGHT);
					break;
				case Key.Down:
					monster.Move(Direction.DOWN);
					break;
				case Key.Up:
					monster.Move(Direction.UP);
					break;
			}
		}
	}

    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouse eventMouse)
		{
			HandleMouseInput(eventMouse);
		}
		else if (@event is InputEventKey eventKey)
		{
			HandleKeyInput(eventKey);
		}
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
