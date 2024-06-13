using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Godot;
using Google.ProtocolBuffers.Serialization;
using NLog;
using y1000.code;
using y1000.Source.Character.Event;
using y1000.Source.Creature;
using y1000.Source.Util;
using FileAccess = Godot.FileAccess;

namespace y1000.Source.Map;

public partial class MapLayer : TileMap, IMap
{
	private GameMap? _gameMap;

	private readonly IDictionary<int, int> _tileIdToSourceId = new Dictionary<int,int>();

	private const int Ground1Zindex = 0;
	private const int Ground2Zindex = 1;
	private const string MAP_DIR = "res://assets/maps/start";

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	private Vector2I _origin;
	
	private const string Object = "object";

	private readonly IDictionary<int, ObjectInfo> _objectInfos = new Dictionary<int, ObjectInfo>();

	private readonly IDictionary<long, Vector2I> _creature2Coordinate = new Dictionary<long, Vector2I>();

	public override void _Ready()
	{
		_gameMap = GameMap.Load("res://assets/maps/start/start.map");
		BuildTileSets();
		BuildObjectSets();
	}
	
	private class ObjectInfo
	{
		public ObjectInfo(Texture2D texture, Vector2 offset)
		{
			Texture = texture;
			Offset = offset;
		}

		public Texture2D Texture { get; }
		
		public Vector2 Offset { get;  }
	
	}

	private void BuildObjectSets()
	{
		var dirpath = "res://assets/maps/start/" + Object;
		var dirAccess = DirAccess.Open(dirpath);
		var subdirs = dirAccess.GetDirectories();
		foreach (var subdir in subdirs)
		{
			var imagePath = dirpath + "/" + subdir + "/image.png";
			if (ResourceLoader.Load(imagePath) is not Texture2D texture)
			{
				continue;
			}
			var jsonString = FileAccess.GetFileAsString(dirpath + "/" + subdir + "/struct.json");
			Object2Json json = Object2Json.FromJsonString(jsonString);
			var offset = new Vector2(json.X, json.Y);
			_objectInfos.TryAdd(subdir.ToInt(), new ObjectInfo(texture, offset));
		}
		dirAccess.Dispose();
		
	}

	public void BindCharacter(Character.CharacterImpl character)
	{
		_origin = character.Coordinate;
		character.WhenCharacterUpdated += OnCharacterEvent;
		PaintMap();
	}

	private void OnCharacterEvent(object? sender, EventArgs args)
	{
		if (sender is Character.CharacterImpl && args is CharacterMoveEventArgs eventArgs &&
		    !_origin.Equals(eventArgs.Coordinate))
		{
			_origin = eventArgs.Coordinate;
			PaintMap();
		}
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
			var path = "res://assets/maps/start/tile/" + id + ".png";
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
		CreateLayer(Object);
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
		_gameMap?.ForeachCell(MapStart, MapEnd, (cell, x, y) => PutObject(Object.Equals(layer)? cell.ObjectId : cell.RoofId, x, y, layer, layerNode));
	}
	

	private void PutObject(int objectId, int x, int y, string layer, Node2D parent)
	{
		if (_objectInfos.TryGetValue(objectId, out var objectInfo))
		{
			int xPos = x * VectorUtil.TileSizeX;
			int yPos = y * VectorUtil.TileSizeY;
			Sprite2D objectSprite = new Sprite2D() { Texture = objectInfo.Texture, Centered = false, Position = new Vector2(xPos, yPos), Offset = objectInfo.Offset, YSortEnabled = true};
			parent.AddChild(objectSprite);
		}
	}

	public bool Movable(Vector2I coordinate)
	{
		if (_gameMap != null && !_gameMap.CanMove(coordinate))
		{
			return false;
		}
		return !_creature2Coordinate.Values.Contains(coordinate);
	}
	
	
	public void Occupy(ICreature creature)
	{
		Free(creature);
		if (_creature2Coordinate.TryAdd(creature.Id, creature.Coordinate))
		{
			LOGGER.Debug("{1} occupied {0}.", creature.Coordinate, creature.Id);
		}
	}

	public void Free(ICreature creature)
	{
		_creature2Coordinate.Remove(creature.Id);
	}
}