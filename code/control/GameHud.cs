using Godot;
using System;
using y1000.code.item;

public partial class GameHud : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InventorySlot slot = GetNode<InventorySlot>("RightSide/GridContainer/Panel");
		slot = GetNode<InventorySlot>("RightSide/GridContainer/Panel3");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void OnMouseEntered()
	{

	}


    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
		IItem item = data.As<Item>();
		if (item == null)
		{
			return false;
		}
		return true;
	}

    public override void _DropData(Vector2 atPosition, Variant data)
    {
		if (!_CanDropData(atPosition, data))
			return;

		var item = data.As<Item>();
		if (item.GetParent() is IItemContainer container)
		{
			container.RemoveItem(item);
		}
		TextureRect rect = new TextureRect();
		rect.Texture = item.Texture;
		rect.Position = atPosition;
		AddChild(rect);
	}


}
