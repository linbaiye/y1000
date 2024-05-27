using y1000.Source.Animation;

namespace y1000.Source.Player;

public interface IPlayerAnimation 
{
    OffsetTexture? HandTexture { get; }
    
    OffsetTexture? ChestTexture { get; }
}