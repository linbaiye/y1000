using System;
using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Map;
using y1000.Source.Networking.Server;
using y1000.Source.Util;

namespace y1000.Source.Creature;

public abstract partial class AbstractCreature : Node2D, ICreature 
{
    public event EventHandler<CreatureMouseClickEventArgs>? MouseClicked;
    
	public event EventHandler<CreatureAnimationDoneEventArgs>? StateAnimationEventHandler;

    private BodySprite? _bodySprite;
    private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
    
    public override void _Ready()
    {
        _bodySprite = GetNode<BodySprite>("Body");
        _bodySprite.Area.GuiInput += MyEvent;
        _bodySprite.SetName(EntityName);
        Log.Debug("Ready for {0}.", EntityName);
    }

    public string EntityName { get; private set; } = "";

    public long Id { get; private set; }

    public IMap Map { get; private set; } = IMap.Empty;
    
    public Direction Direction { get; set; }

    public Vector2I Coordinate => Position.ToCoordinate();

    public abstract OffsetTexture BodyOffsetTexture { get; }
    
    public Vector2 OffsetBodyPosition => Position + BodyOffsetTexture.Offset;
    
    public Vector2 BodyPosition => Coordinate.ToPosition();

    private void MyEvent(InputEvent @event)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left } mouse && mouse.IsPressed())
        {
            MouseClicked?.Invoke(this, new CreatureMouseClickEventArgs(this, mouse));
        }
    }
    
    public void NotifyAnimationFinished(CreatureAnimationDoneEventArgs args)
    {
        StateAnimationEventHandler?.Invoke(this, args);
    }

    public void Delete()
    {
        Map.Free(this);
        QueueFree();
    }
    
    protected void Init(long id, Direction direction, Vector2I coordinate, IMap map, string name)
    {
        Direction = direction;
        Id = id;
        Position = coordinate.ToPosition();
        Map = map;
        EntityName = name;
        _bodySprite?.SetName(name);
        map.Occupy(this);
    }

    public void SetPosition(Vector2I coordinate, Direction direction)
    {
        Direction = direction;
        Position = coordinate.ToPosition();
        Map.Occupy(this);
    }

    public void SetPosition(AbstractPositionMessage positionMessage)
    {
        SetPosition(positionMessage.Coordinate, positionMessage.Direction);
    }
}