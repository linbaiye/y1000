using y1000.Source.Animation;
using y1000.Source.Creature;

namespace y1000.Source.Player;

public sealed class PlayerHurtState : AbstractPlayerState
{
    private PlayerHurtState(CreatureState afterHurt, int elapsedMillis = 0) : base(PlayerBodyAnimation.Male.AnimationMillis(CreatureState.HURT), elapsedMillis)
    {
        AfterHurt = afterHurt;
    }

    public override void Update(Player c, int delta)
    {
        NotifyIfElapsed(c, delta);
    }

    public CreatureState AfterHurt { get; }

    public override CreatureState State => CreatureState.HURT;

    public static PlayerHurtState Hurt(CreatureState after)
    {
        return new PlayerHurtState(after);
    }
}
