using Godot;
using System;

public partial class Item : Node2D
{
	// Called when the node enters the scene tree for the first time.

	private TextureRect textureRect;

	public void LoadTexture(string resName)
	{
		var texture = ResourceLoader.Load("res://sprite/weapon/J42/" + resName + ".png") as Texture2D;
		textureRect.Texture = texture;
	}

	public override void _Ready()
	{
		textureRect = GetNode<TextureRect>("Texture");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public static Item Load(string name)
	{
		var scene = ResourceLoader.Load<PackedScene>("res://item.tscn");
		var item = scene.Instantiate<Item>();
		item._Ready();
		item.LoadTexture(name);
		return item;
	}
}
