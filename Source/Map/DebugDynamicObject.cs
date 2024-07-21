using System;
using System.Collections.Generic;
using System.Text.Json;
using Godot;
using NLog;
using y1000.Source.Character.Event;
using y1000.Source.Util;

namespace y1000.Source.Map;

public partial class DebugDynamicObject : TileMap 
{
	private GameMap? _gameMap;

	private readonly IDictionary<int, int> _tileIdToSourceId = new Dictionary<int,int>();

	private const int Ground1Zindex = 0;
	private const int Ground2Zindex = 1;
	private string _mapName = "fox";
	private const string MapDir = "res://assets/maps/fox/";

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	private Vector2I _origin;
	
	private const string ObjectLayerName = "object";

	private readonly IDictionary<int, ObjectInfo> _objectInfos = new Dictionary<int, ObjectInfo>();

	public override void _Ready()
	{
		_gameMap = GameMap.Load(MapDir + "fox.map");
		BuildTileSets();
		BuildObjectSets();
		_origin = new Vector2I(134, 92);
		GetNode<Sprite2D>("Sprite2D").Position =
			new Vector2(_origin.X * VectorUtil.TileSizeX, _origin.Y * VectorUtil.TileSizeY);
		PaintMap();
	}
	
	private class ObjectInfo
	{
		public ObjectInfo(Texture2D[] texture, Vector2 offset)
		{
			Textures = texture;
			Offset = offset;
		}

		public Texture2D[] Textures { get; }
		
		public Vector2 Offset { get;  }

	}

	private void BuildObjectSets()
	{
		var dirpath = MapDir + ObjectLayerName;
		var dirAccess = DirAccess.Open(dirpath);
		var subdirs = dirAccess.GetDirectories();
		foreach (var subdir in subdirs)
		{
			var jsonString = FileAccess.GetFileAsString(dirpath + "/" + subdir + "/struct.json");
			Object2Json json = Object2Json.FromJsonString(jsonString);
			var offset = new Vector2(json.X, json.Y);
			Texture2D[] texture2Ds = new Texture2D[json.Number];
			for (int i = 0; i < json.Number; i++)
			{
				var imagePath = dirpath + "/" + subdir + "/" + i + ".png";
				if (ResourceLoader.Load(imagePath) is not Texture2D texture)
				{
					LOGGER.Error("Not a texture: {0}.", imagePath);
					throw new Exception();
				}

				texture2Ds[i] = texture;
			}
			_objectInfos.TryAdd(subdir.ToInt(), new ObjectInfo(texture2Ds, offset));
		}
		dirAccess.Dispose();
		
	}

	private void BuildTileSets()
	{
		if (_gameMap == null)
		{
			return;
		}
		var ids = _gameMap.TileIds;
		foreach(var id in ids)
		{
			var path = MapDir + "tile/" + id + ".png";
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
			_tileIdToSourceId.TryAdd(id, sourceId);
		}
	}


	private void PaintMap()
	{
		TileGround();
		CreateLayer(ObjectLayerName);
	}


	private Vector2I MapStart => _origin.Move(-30, -20);
	
	private Vector2I MapEnd => _origin.Move(30, 20);
	

	private void TileGround()
	{
		_gameMap?.ForeachCell(MapStart, MapEnd, (cell, x, y) =>
		{
			if (_tileIdToSourceId.TryGetValue(cell.TileId, out var sourceId))
			{
				SetCell(Ground1Zindex, new Vector2I(x, y), sourceId, new Vector2I(cell.TileNumber, 0));
			}
			if (_tileIdToSourceId.TryGetValue(cell.TileOverId, out var overtileSourceId))
			{
				SetCell(Ground2Zindex, new Vector2I(x, y), overtileSourceId, new Vector2I(cell.TileOverNumber, 0));
			}
		});
	}


	public struct Object2Json
	{
		public Object2Json()
		{
			Width = 0;
			Height = 0;
			X = 0;
			Y = 0;
			Number = 1;
			Delay = 0;
		}

		public byte Version => 2;

		public int Width { get; set; }

		public int Height { get; set; }

		public int X { get; set; }
		public int Y { get; set; }

		public int Number { get; set; }
		
		public int Delay { get; set; }

		public static Object2Json FromJsonString(string jsonString)
		{
			return JsonSerializer.Deserialize<Object2Json>(jsonString);
		}
	}


	private void NotifyCharPosition(Vector2I point)
	{
		if (_gameMap != null && _gameMap.HideRoof(point))  
		{
			GetNode<Node2D>("roof").Hide();
		}
		else 
		{
			GetNode<Node2D>("roof").Show();
		}
	}

	public GameMap? Map => _gameMap;

	private void CreateLayer(string layer)
	{
		var layerNode = GetNode<Node2D>(layer);
		foreach (var child in layerNode.GetChildren())
		{
			layerNode.RemoveChild(child);
			child.QueueFree();
		}
		_gameMap?.ForeachCell(MapStart, MapEnd, (cell, x, y) => PutObject(ObjectLayerName.Equals(layer)? cell.ObjectId : cell.RoofId, x, y, layerNode));
	}


	private AnimatedSprite2D CreateAnimatedSprite2d(ObjectInfo objectInfo, int x, int y)
	{
		SpriteFrames frames = new SpriteFrames();
		for (int i = 0; i < objectInfo.Textures.Length; i++)
		{
			frames.AddFrame("default",objectInfo.Textures[i]);
		}
		return new AnimatedSprite2D()
		{
			Centered = false, Position = new Vector2(x * VectorUtil.TileSizeX, y * VectorUtil.TileSizeY), Offset = objectInfo.Offset, YSortEnabled = true,
			SpriteFrames = frames,
			Autoplay = "default",
		};
	}
	

	private void PutObject(int objectId, int x, int y, Node2D parent)
	{
		if (_objectInfos.TryGetValue(objectId, out var objectInfo))
		{
			int xPos = x * VectorUtil.TileSizeX;
			int yPos = y * VectorUtil.TileSizeY;
			if (objectInfo.Textures.Length > 1)
			{
				var animatedSprite2D = CreateAnimatedSprite2d(objectInfo, x, y);
				parent.AddChild(animatedSprite2D);
			}
			else
			{
				Sprite2D objectSprite = new Sprite2D()
				{
					Texture = objectInfo.Textures[0], Centered = false, Position = new Vector2(xPos, yPos),
					Offset = objectInfo.Offset, YSortEnabled = true
				};
				parent.AddChild(objectSprite);
			}
		}
	}
	
}