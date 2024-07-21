using System;
using System.Collections.Generic;
using System.Text.Json;
using Godot;
using NLog;
using y1000.Source.Character;
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
	private const string MapDir = "res://assets/maps/";

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	private Vector2I _origin;
	
	private const string ObjectLayerName = "object";

	private IDictionary<int, MapObject> _objectInfos = new Dictionary<int, MapObject>();

	private readonly IDictionary<long, Vector2I> _creature2Coordinate = new Dictionary<long, Vector2I>();

	private readonly ISet<string> _animatedSprites = new HashSet<string>();

	private readonly IMapObjectRepository _mapObjectRepository = FilesystemMapObjectRepository.Instance;

	private string _currentMap = "";

	public override void _Ready()
	{
		/*_gameMap = GameMap.Load(MapDir + "start.map");
		BuildTileSets("start");
		BuildObjectSets("start");*/
	}

	private void BuildObjectSets(string name)
	{
		_objectInfos = _mapObjectRepository.LoadObjects(name);
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
			_objectInfos.TryAdd(subdir.ToInt(), new MapObject(texture2Ds, offset, int.Parse(subdir)));
		}
		dirAccess.Dispose();
	}

	private void InitMap(string mapName)
	{
		if (!_currentMap.Equals(mapName))
		{
			_gameMap = GameMap.Load(MapDir + "/" + mapName + "/" + mapName + ".map");
			BuildTileSets(mapName);
			BuildObjectSets(mapName);
			PaintMap();
		}
	}

	public void BindCharacter(CharacterImpl character, string mapName)
	{
		if (mapName.EndsWith(".map"))
		{
			//Remove .map
			mapName = mapName.Substring(0, mapName.Length - 4);
		}
		_origin = character.Coordinate;
		character.WhenCharacterUpdated += OnCharacterEvent;
		InitMap(mapName);
	}

	private void OnCharacterEvent(object? sender, EventArgs args)
	{
		if (sender is CharacterImpl && args is CharacterMoveEventArgs eventArgs &&
		    !_origin.Equals(eventArgs.Coordinate))
		{
			_origin = eventArgs.Coordinate;
			PaintMap();
		}
	}

	private void BuildTileSets(string mapName)
	{
		if (_gameMap == null)
		{
			return;
		}
		var ids = _gameMap.TileIds;
		IDictionary<int, Texture2D> texture2Ds = _mapObjectRepository.LoadTiles(mapName);
		foreach(var id in ids)
		{
			if (!texture2Ds.TryGetValue(id, out var texture))
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

	private void CreateLayer(string layer)
	{
		var layerNode = GetNode<Node2D>(layer);
		foreach (var child in layerNode.GetChildren())
		{
			if (child is not AnimatedSprite2D animatedSprite2D  ||
			    string.IsNullOrEmpty(animatedSprite2D.Name) || 
			    !animatedSprite2D.Name.ToString().StartsWith("obj_"))
			{
				layerNode.RemoveChild(child);
				child.QueueFree();
			}
		}
		_gameMap?.ForeachCell(MapStart, MapEnd, (cell, x, y) => DrawObjectAtCoordinate(ObjectLayerName.Equals(layer)? cell.ObjectId : cell.RoofId, x, y, layerNode));
	}
	
	private AnimatedSprite2D CreateAnimatedSprite2d(MapObject mapObject, int x, int y)
	{
		SpriteFrames frames = new SpriteFrames();
		for (int i = 0; i < mapObject.Textures.Length; i++)
		{
			frames.AddFrame("default",mapObject.Textures[i]);
		}
		var ani = new AnimatedSprite2D()
		{
			Centered = false, Position = new Vector2(x * VectorUtil.TileSizeX, y * VectorUtil.TileSizeY), Offset = mapObject.Offset, YSortEnabled = true,
			SpriteFrames = frames,
			Autoplay = "default",
			Name = mapObject.Name(x, y),
		};
		return ani;
	}
	

	private void DrawObjectAtCoordinate(int objectId, int x, int y, Node2D parent)
	{
		if (_objectInfos.TryGetValue(objectId, out var objectInfo))
		{
			int xPos = x * VectorUtil.TileSizeX;
			int yPos = y * VectorUtil.TileSizeY;
			if (objectInfo.Textures.Length > 1)
			{
				if (!_animatedSprites.Contains(objectInfo.Name(x, y)))
				{
					var animatedSprite2D = CreateAnimatedSprite2d(objectInfo, x, y);
					parent.AddChild(animatedSprite2D);
					_animatedSprites.Add(animatedSprite2D.Name);
				}
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
			// LOGGER.Debug("{1} occupied {0}.", creature.Coordinate, creature.Id);
		}
	}

	public void Free(ICreature creature)
	{
		_creature2Coordinate.Remove(creature.Id);
	}
}