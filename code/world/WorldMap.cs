using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json;
using y1000.code.world;

public partial class WorldMap : TileMap
{
	// Called when the node enters the scene tree for the first time.
	private MapManager? mapManager;

	private int sourceId;

	private IDictionary<int, int> TILE_ID_TO_SOURCE_ID = new Dictionary<int,int>();
	private IDictionary<int, int> OBJECT_ID_TO_SOURCE_ID = new Dictionary<int,int>();

	public override void _Ready()
	{
		mapManager = MapManager.Load("res://assets/maps/prison/prison.map");
		if (mapManager != null)
		{
			BuildTileSets();
			BuildObjects();
			TileMap();
			TileObjects();
		}
		//PackedScene packedScene = new PackedScene();
		//packedScene.Pack(GetTree().CurrentScene);
		//ResourceSaver.Save(packedScene, "res://debugmap.tscn");
	}


	public void BuildObjects()
	{
		if (mapManager == null)
		{
			return;
		}
		var ids = mapManager.ObjectIds;
		foreach(var id in ids)
		{
			var dirpath = "res://assets/maps/prison/object/" + id;
			var path = dirpath + "/image.png";
			if (!Godot.FileAccess.FileExists(path))
			{
				continue;
			}
			var texture = ResourceLoader.Load(path) as Texture2D;
			if (texture == null)
			{
				continue;
			}
			TileSetAtlasSource source = new TileSetAtlasSource() { Texture = texture , TextureRegionSize = new Vector2I(texture.GetWidth(), texture.GetHeight())};
			source.CreateTile(new Vector2I(0, 0));
			using (var fileAccess = Godot.FileAccess.Open(dirpath + "/struct.json", Godot.FileAccess.ModeFlags.Read))
			{
				var jsonString = fileAccess.GetAsText();
				GD.Print("Json : " + jsonString);
				Object2Json json = Object2Json.FromJsonString(jsonString);
				int altId = source.GetAlternativeTileId(new Vector2I(0, 0), 0);
				var tileData = source.GetTileData(new Vector2I(0, 0), altId);
				tileData.TextureOrigin = new Vector2I(json.X, json.Y);
			}
			int sourceId = TileSet.AddSource(source);
			OBJECT_ID_TO_SOURCE_ID.TryAdd(id, sourceId);
		}
	}



	private void BuildTileSets()
	{
		if (mapManager == null)
		{
			return;
		}
		var ids = mapManager.TileIds;
		foreach(var id in ids)
		{
			var path = "res://assets/maps/prison/tile/" + id + ".png";
			if (!Godot.FileAccess.FileExists(path))
			{
				continue;
			}
			var texture = ResourceLoader.Load(path) as Texture2D;
			if (texture == null)
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

	private void TileMap()
	{
		if (mapManager == null)
		{
			return;
		}
		for (int h = 0; h < mapManager.Height; h ++)
		{
			for (int w = 0; w < mapManager.Width; w++)
			{
				var cell = mapManager.GetCell(w, h);
				if (cell == null) 
				{
					continue;
				}
				if (TILE_ID_TO_SOURCE_ID.TryGetValue(cell.Value.TileId, out int sourceId))
				{
					SetCell(0, new Vector2I(w, h), sourceId, new Vector2I(cell.Value.TileNumber, 0));
				}
				if (TILE_ID_TO_SOURCE_ID.TryGetValue(cell.Value.TileOverId, out int overtileSourceId))
				{
					SetCell(1, new Vector2I(w, h), overtileSourceId, new Vector2I(cell.Value.TileOverNumber, 0));
				}
			}
		}
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

	private void TileObjects()
	{
		if (mapManager == null)
		{
			return;
		}
		var ids = mapManager.ObjectIds;

		for (int h = 0; h < mapManager.Height; h++)
		{
			for (int w = 0; w < mapManager.Width; w++)
			{
				var cell = mapManager.GetCell(w, h);
				if (cell == null) 
				{
					continue;
				}
				if (OBJECT_ID_TO_SOURCE_ID.TryGetValue(cell.Value.ObjectId, out int sourceId))
				{
					SetCell(2, new Vector2I(w, h), sourceId, new Vector2I(0, 0));
				}
			}
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
