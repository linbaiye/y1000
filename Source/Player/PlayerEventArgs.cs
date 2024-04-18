using System;

namespace y1000.Source.Player;

public class PlayerEventArgs : EventArgs
{
    public PlayerEventArgs(PlayerMovedEventArgs eventArgs)
    {
        EventArgs = eventArgs;
    }

    public PlayerMovedEventArgs EventArgs { get; }
    
    
}