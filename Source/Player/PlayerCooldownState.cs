using y1000.code;
using y1000.Source.Animation;
using y1000.Source.Creature;

namespace y1000.Source.Player;

public class PlayerCooldownState : AbstractPlayerState
{
    private PlayerCooldownState(int elapsedMillis = 0) : base(PlayerAnimation.Male.AnimationMillis(CreatureState.COOLDOWN),
        elapsedMillis)
    {
    }

    public override void Update(Player c, int delta)
    {
        NotifyIfElapsed(c, delta);
    }

    public static PlayerCooldownState Cooldown(bool male, int elapsed = 0)
    {
        return new PlayerCooldownState(elapsed);
    }

    public override CreatureState State => CreatureState.COOLDOWN;
}