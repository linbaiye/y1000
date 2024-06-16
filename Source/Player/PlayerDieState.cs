using y1000.Source.Creature;

namespace y1000.Source.Player;

public class PlayerDieState  : AbstractPlayerState
{
    public PlayerDieState(int total, int elapsedMillis = 0) : base(total, elapsedMillis)
    {
    }

    public override void Update(PlayerImpl c, int delta)
    {
    }

    public override CreatureState State => CreatureState.DIE;
}