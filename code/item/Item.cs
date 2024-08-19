using System.IO;
using Godot;

namespace y1000.code.item;

public partial class Item : TextureRect, IItem
{
	// Called when the node enters the scene tree for the first time.

	public Item(Texture2D texture)
	{
		Texture = texture;
	}

	public string ItemName => "新罗宝剑";

	public static Item Load(string name)
	{
		var texture = ResourceLoader.Load("res://sprite/item/" + name + ".png") as Texture2D;
		if (texture == null)
		{
			throw new FileNotFoundException();
		}
		return new Item(texture);
	}
}