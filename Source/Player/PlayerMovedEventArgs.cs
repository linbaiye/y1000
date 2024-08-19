using System;

namespace y1000.Source.Player;

public class PlayerMovedEventArgs : EventArgs
{
    public PlayerMovedEventArgs(Player source)
    {
        Source = source;
    }

    public Player Source { get; }
}