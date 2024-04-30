using System;
using Godot;
using NLog;
using y1000.code;
using y1000.code.networking.message;
using y1000.code.player;
using y1000.Source.Map;
using y1000.Source.Networking.Server;
using y1000.Source.Player;
using y1000.Source.Util;

namespace y1000.Source.Creature;

public abstract partial class AbstractCreature : Node2D, ICreature, IBody
{

    public event EventHandler<CreatureMouseClickEventArgs>? MouseClicked;
    
	public event EventHandler<CreatureAnimationDoneEventArgs>? StateAnimationEventHandler;
    
    public override void _Ready()
    {
        var bodyTextContainer = GetNode<BodyTextContainer>("Body/NameContainer");
        bodyTextContainer.GuiInput += MyEvent;
    }

    public string EntityName { get; private set; } = "";

    public long Id { get; private set; }

    public IMap Map { get; private set; } = IMap.Empty;
    
    public Direction Direction { get; set; }

    public Vector2I Coordinate => Position.ToCoordinate();
    
    public abstract OffsetTexture BodyOffsetTexture { get; }

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
        map.Occupy(this);
    }

    public void SetPosition(AbstractPositionMessage positionMessage)
    {
        Direction = positionMessage.Direction;
        Position = positionMessage.Coordinate.ToPosition();
        Map.Occupy(this);
    }
}