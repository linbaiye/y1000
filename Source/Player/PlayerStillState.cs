using System;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Networking;

namespace y1000.Source.Player;

public sealed class PlayerStillState : AbstractPlayerState, IPlayerState
{
    public PlayerStillState(CreatureState state, int elapsedMillis = 0) : base(PlayerAnimation.Male.AnimationMillis(state), elapsedMillis)
    {
        State = state;
    }

    public override void Update(Player c, int delta)
    {
        NotifyIfElapsed(c, delta);
    }

    public override CreatureState State { get; }
    
    public static IPlayerState CreateFrom(PlayerInterpolation playerInterpolation)
    {
        var interpolation = playerInterpolation.Interpolation;
        if (interpolation.State == CreatureState.ATTACK)
        {
            throw new ArgumentException("Attack state not supported.");
        }
        return new PlayerStillState(interpolation.State, interpolation.ElapsedMillis);
    }

}