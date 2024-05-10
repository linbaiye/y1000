using y1000.code;
using y1000.Source.Animation;
using y1000.Source.Entity.Animation;

namespace y1000.Source.Player;

public class PlayerIdleState  : AbstractPlayerState
{
    private PlayerIdleState(int elapsedMillis = 0) : base(PlayerAnimation.Male.AnimationMillis(CreatureState.IDLE), elapsedMillis)
    {
    }

    public override void Update(Player player, int delta)
    {
        NotifyIfElapsed(player, delta);
    }

    public static PlayerIdleState StartFrom(bool male, int elapsedMillis = 0)
    {
        return new PlayerIdleState(elapsedMillis);
    }

    protected override OffsetTexture BodyOffsetTexture(Player player, PlayerAnimation playerAnimation)
    {
        return playerAnimation.OffsetTexture(CreatureState.IDLE, player.Direction, ElapsedMillis);
    }
}