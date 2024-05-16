using y1000.Source.Animation;
using y1000.Source.Creature;

namespace y1000.Source.Player;

public sealed class PlayerHurtState : AbstractPlayerState
{
    private PlayerHurtState(int total, IPlayerState interruptedState, int elapsedMillis = 0) : base(total, elapsedMillis)
    {
        InterruptedState = interruptedState;
    }

    public override void Update(Player c, int delta)
    {
        NotifyIfElapsed(c, delta);
    }

    public IPlayerState InterruptedState { get; }


    public override CreatureState State => CreatureState.HURT;

    public static PlayerHurtState Hurt(IPlayerState state)
    {
        return new PlayerHurtState(PlayerAnimation.Male.AnimationMillis(CreatureState.HURT), state);
    }
    
}
