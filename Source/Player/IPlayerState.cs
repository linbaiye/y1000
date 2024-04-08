using System.Xml.Linq;
using y1000.code.player;

namespace y1000.Source.Player;

public interface IPlayerState
{
    OffsetTexture BodyOffsetTexture(IPlayer player);

    void Update(Player player, long delta);
    
    
    static readonly IPlayerState Empty = EmptyPlayerState.Instance;
    
}