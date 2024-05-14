using System;
using y1000.code;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking;

namespace y1000.Source.Player;

public class PlayerAttackState : AbstractPlayerState
{
    private PlayerAttackState(CreatureState state, int total, int elapsedMillis = 0) : base(total, elapsedMillis)
    {
        State = state;
    }

    public override CreatureState State { get; }
    
    public override void Update(Player c, int delta)
    {
        NotifyIfElapsed(c, delta);
    }
}