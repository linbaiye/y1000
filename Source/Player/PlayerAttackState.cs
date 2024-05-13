using System;
using y1000.code;
using y1000.Source.Animation;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking;

namespace y1000.Source.Player;

public class PlayerAttackState : AbstractPlayerState
{
    private PlayerAttackState(CreatureState state, int total, int elapsedMillis = 0) : base(total, elapsedMillis)
    {
        State = state;
    }
    
    private CreatureState State { get; }
    
    public override void Update(Player c, int delta)
    {
        NotifyIfElapsed(c, delta);
    }
    
    public static PlayerAttackState Quanfa(bool male, bool below50, int elapsedMillis = 0)
    {
        var ani = male ? PlayerAnimation.Male : PlayerAnimation.Female;
        CreatureState state = below50 ? CreatureState.FIST : CreatureState.KICK;
        return new PlayerAttackState(state, ani.AnimationMillis(state), elapsedMillis);
    }

    public static PlayerAttackState FromInterpolation(PlayerInterpolation interpolation)
    {
        if (interpolation.AttackKungFuType == AttackKungFuType.QUANFA)
        {
            return Quanfa(interpolation.Male, interpolation.AttackKungFuBelow50, interpolation.Interpolation.ElapsedMillis);
        }
        throw new NotSupportedException();
    }

    protected override OffsetTexture BodyOffsetTexture(Player player, PlayerAnimation playerAnimation)
    {
        return playerAnimation.OffsetTexture(State, player.Direction, ElapsedMillis);
    }
}