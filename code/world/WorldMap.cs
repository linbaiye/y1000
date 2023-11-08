using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json;
using y1000.code;
using y1000.code.world;

public partial class WorldMap : TileMap
{
	private GameMap? gameMap;

	private readonly IDictionary<int, int> TILE_ID_TO_SOURCE_ID = new Dictionary<int,int>();

	private const int GROUND1_ZINDEX = 0;
	private const int GROUND2_ZINDEX = 1;
	public override void _Ready()
	{
		gameMap = GameMap.Load("res://assets/maps/prison/prison.map");
		BuildTileSets();
		TileGround();
		CreateLayer("object");
		CreateLayer("roof");
		if (!Godot.FileAccess.FileExists("res://debugmap.tscn")) {
			PackedScene packedScene = new PackedScene();
			packedScene.Pack(GetTree().CurrentScene);
			ResourceSaver.Save(packedScene, "res://debugmap.tscn");
		}
        //var objectManager = ObjectManager.Unpack("/Users/ab000785/learn/asset/prison/prisonobj.obj");
        //objectManager?.Dump1("/Users/ab000785/learn/asset/prison/object");
	}

	private void BuildTileSets()
	{
		if (gameMap == null)
		{
			return;
		}
		var ids = gameMap.TileIds;
		foreach(var id in ids)
		{
			var path = "res://assets/maps/prison/tile/" + id + ".png";
			if (!Godot.FileAccess.FileExists(path))
			{
				continue;
			}
			if (ResourceLoader.Load(path) is not Texture2D texture)
			{
				continue;
			}
			TileSetAtlasSource source = new TileSetAtlasSource() { Texture = texture , TextureRegionSize = new Vector2I(32, 24)};
			int width = texture.GetWidth() / 32;
			for (int w = 0; w < width; w++)
			{
				source.CreateTile(new Vector2I(w, 0));
			}
			int sourceId = TileSet.AddSource(source);
			TILE_ID_TO_SOURCE_ID.TryAdd(id, sourceId);
		}
	}


	private void TileGround()
	{
		gameMap?.ForeachCell((cell, x, y) =>
		{
			if (TILE_ID_TO_SOURCE_ID.TryGetValue(cell.TileId, out int sourceId))
			{
				SetCell(GROUND1_ZINDEX, new Vector2I(x, y), sourceId, new Vector2I(cell.TileNumber, 0));
			}
			if (TILE_ID_TO_SOURCE_ID.TryGetValue(cell.TileOverId, out int overtileSourceId))
			{
				SetCell(GROUND2_ZINDEX, new Vector2I(x, y), overtileSourceId, new Vector2I(cell.TileOverNumber, 0));
			}
		});
	}


	public struct Object2Json
	{
		public byte Version => 2;

		public int Width { get; set; }

		public int Height { get; set; }

		public int X { get; set; }
		public int Y { get; set; }

		public static Object2Json FromJsonString(string jsonString)
		{
			return JsonSerializer.Deserialize<Object2Json>(jsonString);
		}
	}


	public void NotifyCharPosition(Vector2I point)
	{
		if (gameMap != null && gameMap.HideRoof(point))  
		{
			GetNode<Node2D>("roof").Hide();
		} else 
		{
			GetNode<Node2D>("roof").Show();
		}
	}


	private void CreateLayer(string layer)
	{
		var layerNode = GetNode<Node2D>(layer);
		gameMap?.ForeachCell((cell, x, y) => PutObject("object".Equals(layer)? cell.ObjectId : cell.RoofId, x, y, layer, layerNode));
	}

	private void PutObject(int objectId, int x, int y, string layer, Node2D parent)
	{
		var dirpath = "res://assets/maps/prison/" + layer + "/" + objectId;
		var imagePath = dirpath + "/image.png";
		if (!Godot.FileAccess.FileExists(imagePath))
		{
			return;
		}
		if (ResourceLoader.Load(imagePath) is not Texture2D texture)
		{
			return;
		}
		using (var fileAccess = Godot.FileAccess.Open(dirpath + "/struct.json", Godot.FileAccess.ModeFlags.Read))
		{
			var jsonString = fileAccess.GetAsText();
			Object2Json json = Object2Json.FromJsonString(jsonString);
			int xPos = x * VectorUtil.TILE_SIZE_X;
			int yPos = y * VectorUtil.TILE_SIZE_Y;
			Sprite2D objectSprite = new Sprite2D() { Texture = texture, Centered = false, Position = new Vector2(xPos, yPos), Offset = new Vector2(json.X, json.Y), YSortEnabled = true};
			parent.AddChild(objectSprite);
		}
	}
}
