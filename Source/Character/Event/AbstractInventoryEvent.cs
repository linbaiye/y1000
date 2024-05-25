using System;
using y1000.Source.Event;

namespace y1000.Source.Character.Event;

public abstract class AbstractInventoryEvent : EventArgs, IUiEvent
{
    protected AbstractInventoryEvent(int slot)
    {
        Slot = slot;
    }

    public int Slot { get; }
    
}