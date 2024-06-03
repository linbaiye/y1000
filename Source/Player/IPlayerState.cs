using System;
using y1000.Source.Creature;
using y1000.Source.Creature.State;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Player;

public interface IPlayerState : ICreatureState<PlayerImpl>
{
    static readonly IPlayerState Empty = EmptyPlayerState.Instance;
    
    CreatureState State { get; }

    void Reset() { }
    
    int ElapsedMillis { get; }
    
    int TotalMillis { get; }
    
    public static IPlayerState Attack(CreatureState state)
    {
        return new PlayerStillState(state);
    }
    
    public static IPlayerState Attack(PlayerAttackMessage message)
    {
        if (message.State == CreatureState.BOW)
        {
            return new PlayerRangedAttackState(message.State, message.TargetId);
        }
        else
        {
            return new PlayerStillState(message.State);
        }
    }


    public static PlayerHurtState Hurt(CreatureState after)
    {
        return PlayerHurtState.Hurt(after);
    }
 
    public static IPlayerState NonHurtState(CreatureState state, Direction direction = Direction.UP, int elapsed = 0)
    {
        switch (state)
        {
            case CreatureState.FLY:
            case CreatureState.WALK:
            case CreatureState.RUN:
            case CreatureState.ENFIGHT_WALK:
                return new PlayerMoveState(state, direction, elapsed);
            case CreatureState.AXE:
            case CreatureState.BOW:
            case CreatureState.FIST:
            case CreatureState.KICK:
            case CreatureState.SWORD:
            case CreatureState.SWORD2H:
            case CreatureState.BLADE:
            case CreatureState.BLADE2H:
            case CreatureState.SPEAR:
            case CreatureState.THROW:
            case CreatureState.IDLE:
            case CreatureState.COOLDOWN:
            case CreatureState.STANDUP:
            case CreatureState.HELLO:
            case CreatureState.DIE:
            case CreatureState.SIT:
                return new PlayerStillState(state, elapsed);
            default:
                throw new NotImplementedException("Can not create state " + state);
        }
    }

    public static IPlayerState CreateFrom(PlayerInterpolation playerInterpolation)
    {
        if (playerInterpolation.Interpolation.State == CreatureState.HURT)
        {
            return PlayerHurtState.Hurt(CreatureState.IDLE);
        }
        return NonHurtState(playerInterpolation.Interpolation.State, playerInterpolation.Interpolation.Direction,
            playerInterpolation.Interpolation.ElapsedMillis);
    }
    
    public static IPlayerState Idle()
    {
        return new PlayerStillState(CreatureState.IDLE);
    }
    
    public static IPlayerState Cooldown()
    {
        return new PlayerStillState(CreatureState.COOLDOWN);
    }
    
}