using System;
using Godot;
using NLog;
using y1000.Source.Audio;
using y1000.Source.Entity;
using y1000.Source.Map;
using y1000.Source.Networking.Server;
using y1000.Source.Util;

namespace y1000.Source.Creature;

public abstract partial class AbstractCreature : AbstractEntity, ICreature
{
    public event EventHandler<CreatureMouseClickEventArgs>? MouseClicked;
    
	public event EventHandler<CreatureAnimationDoneEventArgs>? StateAnimationEventHandler;

    private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

    public IMap Map { get; private set; } = IMap.Empty;
    
    public Direction Direction { get; set; }

    protected override void MyEvent(InputEvent @event)
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

    protected void Delete()
    {
        Log.Debug("Deleting {0}.", Id);
        Map.Free(this);
        QueueFree();
    }
    
    protected void Init(long id, Direction direction, Vector2I coordinate, IMap map, string name)
    {
        Init(id, coordinate, name);
        Direction = direction;
        Map = map;
        map.Occupy(this);
    }

    public void SetPosition(Vector2I coordinate, Direction direction)
    {
        Direction = direction;
        Position = coordinate.ToPosition();
        Map.Occupy(this);
    }
    
	public void ShowLifePercent(int percent)
	{
		GetNode<LifePercentBar>("HealthBar").Display(percent);
	}

    public void HandleHurt(HurtMessage hurtMessage)
    {
        SetPosition(hurtMessage.Coordinate, hurtMessage.Direction);
        ShowLifePercent(hurtMessage.LifePercent);
        PlaySound(hurtMessage.Sound);
    }
    
    public void PlaySound(string name)
    {
        GetNode<CreatureAudio>("Audio").PlaySound(name);
    }
    
    public void SetPosition(AbstractPositionMessage positionMessage)
    {
        SetPosition(positionMessage.Coordinate, positionMessage.Direction);
    }
}