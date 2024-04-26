using System;
using Godot;

namespace y1000.Source.Creature;

public class CreatureMouseClickEventArgs : EventArgs
{
    public CreatureMouseClickEventArgs(ICreature entity, InputEventMouseButton mouseEvent)
    {
        MouseEvent = mouseEvent;
        Creature = entity;
    }

    public ICreature Creature { get; }
    
    public InputEventMouseButton MouseEvent { get; }
    
}