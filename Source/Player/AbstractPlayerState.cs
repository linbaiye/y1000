using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Creature.State;

namespace y1000.Source.Player;

public abstract class AbstractPlayerState : AbstractCreatureState<PlayerImpl>, IPlayerState
{
    protected AbstractPlayerState(int total, int elapsedMillis = 0) : base(total, elapsedMillis)
    {
    }

    protected void NotifyIfElapsed(PlayerImpl player, int delta)
    {
        if (ElapsedMillis >= TotalMillis)
        {
            return;
        }
        if (ElapsedMillis < TotalMillis)
        {
            ElapsedMillis += delta;
        }
        if (ElapsedMillis >= TotalMillis)
        {
            player.NotifyAnimationFinished(new CreatureAnimationDoneEventArgs(this));
        }
    }

    public void Reset()
    {
        ElapsedMillis = 0;
    }

    public abstract CreatureState State { get; }

    
    public override OffsetTexture BodyOffsetTexture(PlayerImpl player)
    {
        var ani = player.IsMale ? PlayerBodyAnimation.Male : PlayerBodyAnimation.Female;
        return ani.OffsetTexture(State, player.Direction, ElapsedMillis);
    }
}