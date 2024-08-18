using System;
using System.Collections.Generic;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Character.Event;
using y1000.Source.Event;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Control.Map;

public partial class MapView : TextureRect
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private static readonly Vector2 WINDOW_VIEW_SIZE = new Vector2(1024, 570);
    private Vector2 _mapSize = Vector2.One;

    private CharacterImpl? _character;
    
    private EventMediator? _eventMediator;

    private MapCreatureView? _characterView;

    private List<MapCreatureView> _npcView;
    
    public override void _Ready()
    {
        Visible = false;
        _characterView = GetNode<MapCreatureView>("Character");
        _characterView.RemoveThemeStyleboxOverride("panel");
        _characterView.AddThemeStyleboxOverride("panel", new StyleBoxFlat() { BgColor = new Color("08f5a5") });
        _npcView = new List<MapCreatureView>();
    }

    public void Initialize(EventMediator eventMediator)
    {
        _eventMediator = eventMediator;
    }

    public void BindCharacter(CharacterImpl character)
    {
        character.WhenCharacterUpdated += CharUpdated;
        _character = character;
        _characterView?.SetName(character.EntityName);
    }

    private void CharUpdated(object? sender, EventArgs eventArgs)
    {
        if (sender is not CharacterImpl)
        {
            return;
        }
        if (eventArgs is CharacterMoveEventArgs && Visible && _mapSize != Vector2.One)
        {
            UpdateCharacterPosition();
        }
        else if (eventArgs is CharacterTeleportedArgs)
        {
            CloseView();
        }
    }

    private void CloseView()
    {
        Visible = false;
        _mapSize = Vector2.One;
        Texture = null;
        foreach (var creatureView in _npcView)
        {
            RemoveChild(creatureView);
            creatureView.QueueFree();
        }
        _npcView.Clear();
    }
    
    private Vector2 ComputeImagePosition(Vector2 imageSize)
    {
        return (WINDOW_VIEW_SIZE - imageSize) / 2;
    }

    private Vector2 ComputeCreaturePosition(Vector2 mapImageSize, Vector2I coordinate, Vector2 squareSize)
    {
        float x = (float)coordinate.X / _mapSize.X;
        float y = (float)coordinate.Y / _mapSize.Y;
        return new Vector2(mapImageSize.X * x - squareSize.X / 2, mapImageSize.Y * y - squareSize.Y / 2);
    }

    private Vector2 ComputeCharPosition(Vector2 imageSize)
    {
        return _character == null || _characterView == null ?
            Vector2.Zero : ComputeCreaturePosition(imageSize, _character.Coordinate, _characterView.Size);
    }

    private void UpdateCharacterPosition()
    {
        if (_characterView != null)
        {
            _characterView.Position = ComputeCharPosition(Texture.GetSize());
        }
    }


    public void DrawNpcs(NpcPositionMessage message)
    {
        if (!Visible)
        {
            return;
        }
        var positions = message.NpcPositions;
        foreach (var npcPosition in positions)
        {
            var mapCreatureView = MapCreatureView.Create(npcPosition.Name);
            AddChild(mapCreatureView);
            mapCreatureView.Position = ComputeCreaturePosition(Texture.GetSize(), npcPosition.Coordinate, mapCreatureView.Size);
            _npcView.Add(mapCreatureView);
        }
    }

    public void Toggle(IMap map)
    {
        if (Visible)
        {
            CloseView();
            return;
        }
        var image = ImageTexture.CreateFromImage(Image.LoadFromFile("assets/maps/" + map.MapName + ".bmp"));
        if (image != null)
        {
            Texture = image;
            Position = ComputeImagePosition(Texture.GetSize());
            _mapSize = map.MapSize;
            Visible = true;
            UpdateCharacterPosition();
            Logger.Debug("Map position {0}, Size {1}.", Position, Texture.GetSize());
            _eventMediator?.NotifyServer(ClientSimpleCommandEvent.NpcPosition);
        }
    }
}