using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Character.Event;
using y1000.Source.DynamicObject;
using y1000.Source.Entity;
using y1000.Source.Util;

namespace y1000.Source.Map;

public partial class MapLayer : TileMap, IMap
{
	private GameMap? _gameMap;

	private readonly IDictionary<int, int> _tileIdToSourceId = new Dictionary<int,int>();

	private const int Ground1Zindex = 0;
	private const int Ground2Zindex = 1;
	//private const int ItemZindex = 2;
	private const int EntityZindex = 3;
	private const int RoofZindex = 4;
	private const string MapDir = "res://assets/maps/";

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	private Vector2I _origin;
	
	private const string ObjectLayerName = "object";
	private const string RoofLayerName = "roof";

	private IDictionary<int, MapObject> _mapObjectInfos = new Dictionary<int, MapObject>();
	private IDictionary<int, MapObject> _mapRoofInfos = new Dictionary<int, MapObject>();

	private readonly IDictionary<long, ISet<Vector2I>> _entityCoordinates= new Dictionary<long, ISet<Vector2I>>();

	private readonly ISet<string> _animatedObjectSprites = new HashSet<string>();

	private readonly IMapObjectRepository _mapObjectRepository = IMapObjectRepository.Instance;

	private const int CameraLimitOffset = 6;

	private void InitMap(string mapName, string tileName, string objName, string rofName)
	{
		if (mapName.EndsWith(".map"))
		{
			mapName = mapName.Substring(0, mapName.Length - 4);
		}
		_entityCoordinates.Clear();
		if (_gameMap == null || !_gameMap.Name.Equals(mapName))
		{
			_gameMap = GameMap.Load(MapDir + "/" +  mapName + ".map");
			_animatedObjectSprites.Clear();
			_tileIdToSourceId.Clear();
			Clear();
			ClearLayer(ObjectLayerName);
			if (tileName.EndsWith("til.til"))
				tileName = tileName.Substring(0, tileName.Length - 7);
			else if (tileName.EndsWith(".til"))
				tileName = tileName.Substring(0, tileName.Length - 4);
			BuildTileSets(tileName);
			if (objName.EndsWith("obj.obj"))
				objName = objName.Substring(0, objName.Length - 7);
			else if (objName.EndsWith(".obj"))
				objName = objName.Substring(0, objName.Length - 4);
			_mapObjectInfos = _mapObjectRepository.LoadObjects(objName);
			if (rofName.EndsWith("rof.obj"))
				rofName = rofName.Substring(0, rofName.Length - 7);
			else if (rofName.EndsWith("obj.obj"))
				rofName = rofName.Substring(0, rofName.Length - 7);
			else if (rofName.EndsWith(".obj"))
				rofName= rofName.Substring(0, rofName.Length - 4);
			if (_mapObjectRepository.HasRoof(rofName))
				_mapRoofInfos = _mapObjectRepository.LoadRoof(rofName);
		}
	}

	public void BindCharacter(CharacterImpl character, string mapName, string tileName, string objName, string rofName)
	{
		InitMap(mapName, tileName, objName, rofName);
		_origin = character.Coordinate;
		PaintMap();
		character.WhenCharacterUpdated += OnCharacterEvent;
		PutCameraLimit(character);
	}

	private void PutCameraLimit(CharacterImpl character)
	{
		if (_gameMap == null)
		{
			return;
		}

		var leftTopLimit = ComputeCameraLeftTopLimit(_gameMap);
		var rightBottomLimit = ComputeCameraRightBottomLimit(_gameMap);
		character.LimitCamera(leftTopLimit, rightBottomLimit);
	}

	private Vector2I ComputeCameraRightBottomLimit(GameMap map)
	{
		if (map.Name.Equals("start"))
		{
			int x = map.Width > CameraLimitOffset ? map.Width - CameraLimitOffset : map.Width;
			int y = map.Height > CameraLimitOffset ? map.Height - CameraLimitOffset : map.Height;
			return new Vector2I(x * VectorUtil.TileSizeX, y * VectorUtil.TileSizeY);
		}
		return new Vector2I(map.Width * VectorUtil.TileSizeX, map.Height * VectorUtil.TileSizeY);
	}

	private Vector2I ComputeCameraLeftTopLimit(GameMap map)
	{
		if (map.Name.Equals("start"))
		{
			int x = map.Width > CameraLimitOffset ? CameraLimitOffset : map.Width;
			int y = map.Height > CameraLimitOffset ? CameraLimitOffset : map.Height;
			return new Vector2I(x * VectorUtil.TileSizeX, y * VectorUtil.TileSizeY);
		}
		return new Vector2I(0, 0);
	}
	
	private void NotifyIfReachEdge(CharacterImpl character, Vector2I coordinate)
	{
		if (_gameMap == null)
		{
			return;
		}
		int range = 20;
		if (coordinate.X < range || coordinate.X > _gameMap.Width -range 
		    || coordinate.Y < range|| coordinate.Y > _gameMap.Height - range)
		{
			character.ReachEdge();
		}
	}
	private void OnCharacterEvent(object? sender, EventArgs args)
	{
		if (sender is not CharacterImpl character)
		{
			return;
		}
		if (args is CharacterMoveEventArgs eventArgs && !_origin.Equals(eventArgs.Coordinate))
		{
			_origin = eventArgs.Coordinate;
			PaintMap();
			HideRoofIfNeed();
		}
		else if (args is CharacterTeleportedArgs teleportedArgs)
		{
			InitMap(teleportedArgs.NewMap, teleportedArgs.Tile, teleportedArgs.Object, teleportedArgs.Roof);
			_origin = teleportedArgs.Coordinate;
			PaintMap();
			HideRoofIfNeed();
			PutCameraLimit(character);
		}
	}

	private void BuildTileSets(string tileFile)
	{
		if (_gameMap == null)
		{
			return;
		}
		IDictionary<int, Texture2D> texture2Ds = _mapObjectRepository.LoadTiles(tileFile);
		var ids = _gameMap.TileIds;
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

	private void PaintMap()
	{
		TileGround();
		RefreshObjectLayer();
		RefreshRoofLayer();
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
	
	private void HideRoofIfNeed()
	{
		if (_gameMap != null && _gameMap.HideRoof(_origin))  
		{
			GetNode<Node2D>(RoofLayerName).Hide();
		}
		else 
		{
			GetNode<Node2D>(RoofLayerName).Show();
		}
	}

	private void ClearLayer(string layer)
	{
		var layerNode = GetNode<Node2D>(layer);
		foreach (var child in layerNode.GetChildren())
		{
			layerNode.RemoveChild(child);
			child.QueueFree();
		}
	}

	private void RefreshRoofLayer()
	{
		var layerNode = GetNode<Node2D>(RoofLayerName);
		foreach (var child in layerNode.GetChildren())
		{
			layerNode.RemoveChild(child);
			child.QueueFree();
		}
		_gameMap?.ForeachCell(MapStart, MapEnd, (cell, x, y) => DrawRoofAtCoordinate(cell.RoofId, x, y, layerNode));
	}

	private void RefreshObjectLayer()
	{
		var layerNode = GetNode<Node2D>(ObjectLayerName);
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
		_gameMap?.ForeachCell(MapStart, MapEnd, (cell, x, y) => DrawObjectAtCoordinate(cell.ObjectId, x, y, layerNode));
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
			Centered = false, Position = new Vector2(x * VectorUtil.TileSizeX, y * VectorUtil.TileSizeY), Offset = mapObject.Offset,
			YSortEnabled = true,
			ZIndex = EntityZindex,
			ZAsRelative = false,
			SpriteFrames = frames,
			Autoplay = "default",
			Name = mapObject.Name(x, y),
		};
		return ani;
	}

	private void DrawRoofAtCoordinate(int rofId, int x, int y, Node2D parent)
	{
		if (_mapRoofInfos.TryGetValue(rofId, out var rofInfo))
		{
			int xPos = x * VectorUtil.TileSizeX;
			int yPos = y * VectorUtil.TileSizeY;
			Sprite2D objectSprite = new Sprite2D()
			{
				Texture = rofInfo.Textures[0], Centered = false, Position = new Vector2(xPos, yPos),
				Offset = rofInfo.Offset, YSortEnabled = true, ZIndex = RoofZindex
			};
			parent.AddChild(objectSprite);
		}
	}
	
	private void DrawObjectAtCoordinate(int objectId, int x, int y, Node2D parent)
	{
		if (_mapObjectInfos.TryGetValue(objectId, out var objectInfo))
		{
			int xPos = x * VectorUtil.TileSizeX;
			int yPos = y * VectorUtil.TileSizeY;
			if (objectInfo.Textures.Length > 1)
			{
				if (!_animatedObjectSprites.Contains(objectInfo.Name(x, y)))
				{
					var animatedSprite2D = CreateAnimatedSprite2d(objectInfo, x, y);
					parent.AddChild(animatedSprite2D);
					_animatedObjectSprites.Add(animatedSprite2D.Name);
				}
			}
			else
			{
				Sprite2D objectSprite = new Sprite2D()
				{
					Texture = objectInfo.Textures[0], Centered = false, Position = new Vector2(xPos, yPos),
					Offset = objectInfo.Offset, YSortEnabled = true, ZIndex = EntityZindex, ZAsRelative = false
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
		return !_entityCoordinates.Values.Any(s => s.Contains(coordinate));
	}


	
	public void Occupy(IEntity entity)
	{
		Free(entity);
		_entityCoordinates.TryAdd(entity.Id, new HashSet<Vector2I>() { entity.Coordinate });
	}


	public void Free(IEntity entity)
	{
		_entityCoordinates.Remove(entity.Id);
	}

	public void Occupy(GameDynamicObject dynamicObject)
	{
		_entityCoordinates.TryAdd(dynamicObject.Id, new HashSet<Vector2I>(dynamicObject.Coordinates));
	}
	
	public void Free(GameDynamicObject dynamicObject)
	{
		_entityCoordinates.Remove(dynamicObject.Id);
	}

	public Vector2I MapSize => _gameMap != null ? new Vector2I(_gameMap.Width, _gameMap.Height) : Vector2I.Zero;
	public string MapName => _gameMap != null ? _gameMap.Name : "";
}