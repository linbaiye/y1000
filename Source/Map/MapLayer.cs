using System.Collections.Generic;
using System.Text.Json;
using Godot;
using NLog;
using y1000.code;
using y1000.Source.Character.Event;

namespace y1000.Source.Map;

public partial class MapLayer : TileMap
{
	private GameMap? _gameMap;

	private readonly IDictionary<int, int> _tileIdToSourceId = new Dictionary<int,int>();

	private const int Ground1Zindex = 0;
	private const int Ground2Zindex = 1;
	private const string MAP_DIR = "res://assets/maps/start";

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	private Vector2I _origin;
	public override void _Ready()
	{
		_gameMap = GameMap.Load("res://assets/maps/start/start.map");
		BuildTileSets();
	}

	public void BindCharacter(Character.Character character)
	{
		_origin = character.Coordinate;
		character.WhenCharacterUpdated += OnCharacterEvent;
		PaintMap();
	}

	private void OnCharacterEvent(object? sender, AbstractCharacterEventArgs args)
	{
		if (sender is Character.Character character && !character.Coordinate.Equals(_origin))
		{
			_origin = character.Coordinate;
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
		CreateLayer("object");
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
		_gameMap?.ForeachCell(MapStart, MapEnd, (cell, x, y) => PutObject("object".Equals(layer)? cell.ObjectId : cell.RoofId, x, y, layer, layerNode));
	}

	private void PutObject(int objectId, int x, int y, string layer, Node2D parent)
	{
		var dirpath = "res://assets/maps/start/" + layer + "/" + objectId;
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
			int xPos = x * VectorUtil.TileSizeX;
			int yPos = y * VectorUtil.TileSizeY;
			Sprite2D objectSprite = new Sprite2D() { Texture = texture, Centered = false, Position = new Vector2(xPos, yPos), Offset = new Vector2(json.X, json.Y), YSortEnabled = true};
			parent.AddChild(objectSprite);
		}
	}
}