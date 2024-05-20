using y1000.Source.Animation;
using y1000.Source.Creature;

namespace y1000.Source.Player;

public sealed class PlayerStillState : AbstractPlayerState, IPlayerState
{
    public PlayerStillState(CreatureState state, int elapsedMillis = 0) : base(PlayerBodyAnimation.Male.AnimationMillis(state), elapsedMillis)
    {
        State = state;
    }

    public override void Update(Player c, int delta)
    {
        NotifyIfElapsed(c, delta);
    }

    public override CreatureState State { get; }
}