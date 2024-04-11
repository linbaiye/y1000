namespace y1000.Source.Player;

public class PlayerMovedEvent
{
    public PlayerMovedEvent(Player source)
    {
        Source = source;
    }

    public Player Source { get; }
    
}