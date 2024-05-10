using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Creature.State;
using y1000.Source.Entity.Animation;

namespace y1000.Source.Player;

public abstract class AbstractPlayerState : AbstractCreatureState<Player>, IPlayerState
{
    protected AbstractPlayerState(int total, int elapsedMillis = 0) : base(total, elapsedMillis)
    {
    }

    protected void NotifyIfElapsed(Player player, int delta)
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

    protected abstract OffsetTexture BodyOffsetTexture(Player player, PlayerAnimation playerAnimation);

    
    public override OffsetTexture BodyOffsetTexture(Player player)
    {
        var ani = player.IsMale ? PlayerAnimation.Male : PlayerAnimation.Female;
        return BodyOffsetTexture(player, ani);
    }
}