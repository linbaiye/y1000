using y1000.code;
using y1000.code.player;

namespace y1000.Source.Player;

public interface IPlayer
{
    bool IsMale { get; }
    
    Direction Direction { get; }

    OffsetTexture BodyOffsetTexture { get; }
}