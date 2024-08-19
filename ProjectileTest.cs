using System.Collections.Generic;
using Godot;
using y1000.Source.Creature;

namespace y1000;

public partial class ProjectileTest : Sprite2D
{
	// Called when the node enters the scene tree for the first time.

	private static readonly Dictionary<Direction, Vector2> _vector2s = new Dictionary<Direction, Vector2>()
	{
		{ Direction.RIGHT, new Vector2(1, 0) },
		{ Direction.LEFT, new Vector2(-1, 0) },
		{ Direction.UP, new Vector2(0, -1) },
		{ Direction.DOWN, new Vector2(0, 1) },
	};

	private readonly Direction _direction = Direction.RIGHT;
	
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton button && button.IsPressed())
		{
			var globalMousePosition = GetGlobalMousePosition();
			var vec = (globalMousePosition - GlobalPosition).Normalized();
			//var angleTo = vec.AngleTo(_vector2s.GetValueOrDefault(_direction, new Vector2()));
			var angleTo = vec.Angle();
			//var angleTo = vec.AngleTo(_vector2s.GetValueOrDefault(_direction, new Vector2()));
			Rotation = angleTo;
			GD.Print(angleTo);
		}
	}
}