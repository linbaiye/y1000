using System;
using Godot;
using y1000.code.item;

namespace y1000.code.world;

public partial class World : Node2D, IItemContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	/*public override void _Input(InputEvent @event)
{
	if (@event is InputEventMouseButton button)
	{
		if (button.DoubleClick)
		{
			GD.Print("rteceived in world: " + button.AsText());
		}
	}
}*/

	public bool RemoveItem(IItem item)
	{
		throw new NotImplementedException();
	}

	public void PutItem(IItem item)
	{
		throw new NotImplementedException();
	}

	public bool ContainsItem(IItem item)
	{
		throw new NotImplementedException();
	}

	public bool AtCursor()
	{
		var map = GetNode<Godot.TileMap>("Map");
		var mousePos = GetTree().Root.GetMousePosition();
		var end = map.GetUsedRect().End;
		return mousePos.X <= end.X * VectorUtil.TileSizeX && mousePos.X >= 0 &&
		       mousePos.Y <= end.Y * VectorUtil.TileSizeY && mousePos.Y >= 0;
	}

}