using y1000.Source.Animation;
using y1000.Source.Creature;

namespace y1000.Source.Player;

public sealed class PlayerHurtState : AbstractPlayerState
{
    public PlayerHurtState(int total, int elapsedMillis = 0) : base(total, elapsedMillis)
    {
    }

    public override void Update(Player c, int delta)
    {
        NotifyIfElapsed(c, delta);
    }

    protected override CreatureState State => CreatureState.HURT;

    public static PlayerHurtState Hurt()
    {
        return new PlayerHurtState(PlayerAnimation.Male.AnimationMillis(CreatureState.HURT));
    }
}
