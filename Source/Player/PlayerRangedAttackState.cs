
using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature;

namespace y1000.Source.Player;

public class PlayerRangedAttackState : AbstractPlayerState
{
    public PlayerRangedAttackState(CreatureState state, long targetId, int elapsedMillis = 0) : base(PlayerBodyAnimation.Male.AnimationMillis(state), elapsedMillis)
    {
        TargetId = targetId;
        State = state;
    }
    
    private long TargetId { get; }
    
    public override void Update(PlayerImpl c, int delta)
    {
        NotifyIfElapsed(c, delta);
        if (ElapsedMillis >= TotalMillis)
        {
            c.EmitRangedAttackEvent(TargetId);
        }
    }
    public override CreatureState State { get; }
}