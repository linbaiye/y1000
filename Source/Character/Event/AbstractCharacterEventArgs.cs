using System;
using y1000.Source.Input;

namespace y1000.Source.Character.Event;

public abstract class AbstractCharacterEventArgs : EventArgs
{
    protected AbstractCharacterEventArgs(IClientEvent @event)
    {
        Event = @event;
    }

    public IClientEvent Event { get; }
    
}