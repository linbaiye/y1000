using Godot;
using System;

public partial class Slot : Panel
{
	// Called when the node enters the scene tree for the first time.
	private Item? loadedItem = null;
	private string? name;
	public override void _Ready()
	{
		//GuiInput += OnInputEvent;
		//GD.Print("ready");
	}

	public void LoadItem(string name)
	{
		if (loadedItem != null)
		{
			return;
		}
		this.name = name;
		loadedItem = Item.Load(name);
		AddChild(loadedItem);
	}


    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
		return true;
    }



    public override Variant _GetDragData(Vector2 atPosition)
    {
		//GD.Print("Get drag");
		if (loadedItem == null)
		{
			return new Variant();
		}
		var ret = Variant.CreateFrom(loadedItem);
		loadedItem = null;
		return ret;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
		Item newItem = data.As<Item>();
		if (newItem != null)
		{
			if (loadedItem != null)
			{
				RemoveChild(loadedItem);
			}
			newItem.GetParent().RemoveChild(newItem);
			AddChild(newItem);
			loadedItem = newItem;
		}
	}


	/*public void OnInputEvent(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton button)
		{
			if (button.IsPressed() && button.ButtonIndex == MouseButton.Left)
			{
				//GD.Print("pressed");
				//GD.Print(name);
			} else if (button.IsReleased() && button.ButtonIndex == MouseButton.Left)
			{
				//GD.Print("realeased");
			}
			GD.Print(button.AsText());
			GD.Print(name);
		}
	}*/



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
