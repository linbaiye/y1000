using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
using y1000.code.item;

public partial class GridContainer : Godot.GridContainer
{

	public override void _Ready()
	{
		var all = GetChildren();
		//all[0].Connect("gui_input", Callable.From<InputEvent, int>((e, i) => GD.Print(0)));
		//Action<InputEvent> action = (e) => GD.Print("event " + 1);
		/*int k = 0;
			((PanelContainer)all[0]).GuiInput += (e) => {
				GD.Print("world");
				ButtonInput(e, k);
			};
			++k;*/
		int i = 0;
		while (i < all.Count)
		{
			Array<int> attr = new Array<int>(){i};
			((InventorySlot)all[i]).GuiInput += (e) => ButtonInput(e, attr);
			i++;
		}
	}

	/*for (int i = 0; i < all.Count; i++)
	{
		var c = all[i];
		((PanelContainer)all[i]).GuiInput += (e) => ButtonInput(e, new Godot.Collections.Array(){i});
		//((PanelContainer)all[1]).GuiInput += (e) => ButtonInput(e, i);
	}*/


	private class ItemContainerNode
	{
		private readonly IItemContainer container;

		private readonly int deepth;

        public ItemContainerNode(IItemContainer container, int deepth)
        {
            this.container = container;
            this.deepth = deepth;
        }

		public IItemContainer Container => container;

		public int Deepth => deepth;
    }


	private void CollectContainers(Node node, int depth, List<ItemContainerNode> containers)
	{
		if (node is IItemContainer container)
		{
			if (container.AtCursor())
			{
				containers.Add(new ItemContainerNode(container, depth));
			}
		}
		foreach(var child in node.GetChildren())
		{
			CollectContainers(child, depth + 1, containers);
		}
	}

	private IItemContainer? FindContainerAtCursor()
	{
		var root = GetTree().Root;
		List<ItemContainerNode> containerNodes = new();
		CollectContainers(root, 0, containerNodes);
		int maxDepth = -1;
		IItemContainer? result = null;
		foreach (var node in containerNodes)
		{
			if (node.Deepth > maxDepth)
			{
				result = node.Container;
			}
		}
		return result;
	}


	public void ButtonInput(InputEvent inputEvent, Array<int> attr)
	{
		int number = attr[0];
		if (inputEvent is InputEventMouseButton button)
		{
			if (button.IsPressed() && button.ButtonIndex == MouseButton.Left)
			{
				GD.Print("Clicked on" + number);
			}
			else if (button.IsReleased() && button.ButtonIndex == MouseButton.Left)
			{
				var container = FindContainerAtCursor();
				if (container != null)
				{
					GD.Print("Got container");
				}
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
