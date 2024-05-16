using System;
using y1000.Source.Creature;
using y1000.Source.Creature.State;

namespace y1000.Source.Player;

public interface IPlayerState : ICreatureState<Player>
{
    static readonly IPlayerState Empty = EmptyPlayerState.Instance;
    
    CreatureState State { get; }

    void Reset() { }
    
    public static IPlayerState Attack(CreatureState state)
    {
        return new PlayerStillState(state);
    }


    public static IPlayerState Hurt(IPlayerState current)
    {
        return PlayerHurtState.Hurt(current);
    }
    
    public static IPlayerState Walk(Direction direction)
    {
        return PlayerMoveState.WalkTowards(direction);
    }
    
    public static IPlayerState Run(Direction direction)
    {
        return PlayerMoveState.RunTowards(direction);
    }
    
    public static IPlayerState Fly(Direction direction)
    {
        return PlayerMoveState.FlyTowards(direction);
    }

    public static IPlayerState Move(CreatureState movingState, Direction direction)
    {
        return movingState switch
        {
            CreatureState.WALK => Walk(direction),
            CreatureState.RUN => Run(direction),
            CreatureState.FLY => Run(direction),
            CreatureState.ENFIGHT_WALK => Run(direction),
            _ => throw new NotImplementedException("Bad moving state: " + movingState)
        };
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