using y1000.code.player;

namespace y1000.Source.Player;

public class EmptyPlayerState : IPlayerState
{
    public static readonly EmptyPlayerState Instance = new EmptyPlayerState();
    private EmptyPlayerState (){}
    public OffsetTexture BodyOffsetTexture(IPlayer player)
    {
        throw new System.NotImplementedException();
    }

    public void Update(Player player, long delta)
    {
        throw new System.NotImplementedException();
    }
}