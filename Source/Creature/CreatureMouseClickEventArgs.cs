using System;
using Godot;

namespace y1000.Source.Creature;

public class CreatureMouseClickEventArgs : EventArgs
{
    public CreatureMouseClickEventArgs(long id, InputEventMouseButton mouseEvent)
    {
        Id = id;
        MouseEvent = mouseEvent;
    }

    public long Id { get; }
    
    public InputEventMouseButton MouseEvent { get; }
    
    
}