using Godot;
using System;
using System.Collections.Generic;
using test.cide;

public partial class Body : AbstractBodyPart
{

	private static readonly IDictionary<State, IDictionary<Direction, int>> OFFSET = new Dictionary<State, IDictionary<Direction, int>>() {
		{State.WALK, new Dictionary<Direction, int>() {
			{ Direction.UP, 0},
			{ Direction.UP_RIGHT, 6},
			{ Direction.RIGHT, 12},
			{ Direction.DOWN_RIGHT, 18},
			{ Direction.DOWN, 24},
			{ Direction.DOWN_LEFT, 30},
			{ Direction.LEFT, 36},
			{ Direction.UP_LEFT, 42},
		}},
		{State.IDLE, new Dictionary<Direction, int>() {
			{ Direction.UP, 48},
			{ Direction.UP_RIGHT, 51},
			{ Direction.RIGHT, 54},
			{ Direction.DOWN_RIGHT, 57},
			{ Direction.DOWN, 60},
			{ Direction.DOWN_LEFT, 63},
			{ Direction.LEFT, 66},
			{ Direction.UP_LEFT, 69},
		}},
	};


	public override void _Ready()
	{
		SpriteContainer = SpriteContainer.Load("sprite/char/N02");
	}

    protected override Vector2 GetPosition(int spriteNumber)
    {
		if (OFFSET.TryGetValue(State, out IDictionary<Direction, int> map)) {
			if (map.TryGetValue(Direction, out int start)) {
				return SpriteContainer.GetOffset(start + spriteNumber);
			} 
		}
		throw new InvalidOperationException();
	}

    protected override Texture2D GetTexture(int spriteNumber)
    {
		if (OFFSET.TryGetValue(State, out IDictionary<Direction, int> map)) {
			if (map.TryGetValue(Direction, out int start)) {
				return SpriteContainer.GetTexture(start + spriteNumber);
			} 
		}
        throw new NotImplementedException();
    }
}
