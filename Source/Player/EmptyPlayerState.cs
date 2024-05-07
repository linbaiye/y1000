using Godot;
using y1000.code.player;
using y1000.Source.Entity.Animation;

namespace y1000.Source.Player;

public class EmptyPlayerState : IPlayerState
{
    public static readonly EmptyPlayerState Instance = new EmptyPlayerState();
    private EmptyPlayerState (){}
    public OffsetTexture BodyOffsetTexture(Player player)
    {
        throw new System.NotImplementedException();
    }

    public bool Contains(Player c, Vector2 position)
    {
        throw new System.NotImplementedException();
    }

    public void Update(Player c, long delta)
    {
        throw new System.NotImplementedException();
    }
}