using y1000.code;
using y1000.Source.Animation;
using y1000.Source.Creature;

namespace y1000.Source.Player;

public class PlayerIdleState : AbstractPlayerState
{
    private PlayerIdleState(int elapsedMillis = 0) : base(PlayerAnimation.Male.AnimationMillis(CreatureState.IDLE), elapsedMillis)
    {
    }

    public override void Update(Player player, int delta)
    {
        NotifyIfElapsed(player, delta);
    }


    public override CreatureState State => CreatureState.IDLE;
}