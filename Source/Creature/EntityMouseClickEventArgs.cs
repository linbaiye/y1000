using System;
using Godot;
using y1000.Source.Entity;

namespace y1000.Source.Creature;

public class EntityMouseClickEventArgs : EventArgs
{
    public EntityMouseClickEventArgs(IEntity entity, InputEventMouseButton mouseEvent)
    {
        MouseEvent = mouseEvent;
        Entity = entity;
    }

    public IEntity Entity { get; }
    
    public InputEventMouseButton MouseEvent { get; }
    
}