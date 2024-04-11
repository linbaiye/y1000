using System;

namespace y1000.Source.Player;

public class PlayerEventArgs : EventArgs
{
    public PlayerEventArgs(PlayerMovedEvent @event)
    {
        Event = @event;
    }

    public PlayerMovedEvent Event { get; }
    
    
}