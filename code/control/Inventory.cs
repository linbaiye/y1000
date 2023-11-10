using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using y1000.code.item;

namespace y1000.code
{

	public partial class Inventory : Godot.GridContainer
	{

		public override void _Ready()
		{
			var all = GetChildren();
			for (int i = 0; i < all.Count; i++)
			{
				if (all[i] is InventorySlot slot)
				{
					Array<int> attr = new Array<int>() { i };
					slot.GuiInput += (e) => OnInputEvent(e, attr);
					slot.LoadItem("00008" + i);
				}
			}
		}


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
			foreach (var child in node.GetChildren())
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


		public void OnInputEvent(InputEvent inputEvent, Array<int> attr)
		{
			int number = attr[0];
			if (inputEvent is InputEventMouseButton button)
			{
				if (button.ButtonIndex == MouseButton.Left)
				{
					if (button.DoubleClick)
					{
						GD.Print("Recieveid double in inventory.");
					}
					if (button.IsPressed())
					{
						GD.Print("Dragged " + number);
					}
					else if (button.IsReleased())
					{
						var container = FindContainerAtCursor();
						if (container != null)
						{
							GD.Print("Got container");
						}
					}
				}
			}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}