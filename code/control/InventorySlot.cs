using Godot;
using y1000.code.item;

namespace y1000.code.control;

public partial class InventorySlot: Panel, IItemContainer
{
	// Called when the node enters the scene tree for the first time.
	private item.Item? loadedItem = null;

	public void LoadItem(string name)
	{
		if (loadedItem != null)
		{
			return;
		}
		loadedItem = item.Item.Load(name);
		var textureRect = GetNode<TextureRect>("TextureRect");
		textureRect.Texture = loadedItem.Texture;
	}


/*
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
		if (data.VariantType == Variant.Type.Nil)
		{
			return false;
		}
		return true;
    }


    public override Variant _GetDragData(Vector2 atPosition)
    {
		if (loadedItem == null)
		{
			return new Variant();
		}
		return Variant.CreateFrom(loadedItem);
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
		Item newItem = data.As<Item>();
		if (data.As<Item>() != null && newItem.GetParent() is IItemContainer oridinalContainer)
		{
			oridinalContainer.RemoveItem(newItem);
			if (loadedItem != null)
			{
				RemoveChild(loadedItem);
				oridinalContainer.PutItem(loadedItem);
			}
			AddChild(newItem);
			loadedItem = newItem;
		}
	}*/


	public void PutItem(IItem item)
	{
		if (item is item.Item target && loadedItem == null)
		{
			loadedItem = target;
			AddChild(target);
		}
	}

	public bool RemoveItem(IItem item)
	{
		if (item is item.Item target && loadedItem != null)
		{
			if (target == loadedItem)
			{
				RemoveChild(target);
				loadedItem = null;
				return true;
			}
		}
		return false;
	}


	public bool AtCursor()
	{
		return GetGlobalRect().HasPoint(GetGlobalMousePosition());
	}



	public bool ContainsItem(IItem item)
	{
		return (item is item.Item) && item == loadedItem;
	}
}