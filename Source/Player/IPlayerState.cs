using System;
using System.Xml.Linq;
using y1000.code;
using y1000.code.player;
using y1000.Source.Creature;
using y1000.Source.Creature.State;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Player;

public interface IPlayerState : ICreatureState<Player>
{
    static readonly IPlayerState Empty = EmptyPlayerState.Instance;

    static IPlayerState CreateFrom(PlayerInterpolation playerInterpolation)
    {
        var interpolation = playerInterpolation.Interpolation;
        return interpolation.State switch
        {
            CreatureState.IDLE => PlayerIdleState.StartFrom(playerInterpolation.Male, interpolation.ElapsedMillis),
            CreatureState.FLY => PlayerMoveState.FlyTowards(playerInterpolation.Male, interpolation.Direction, interpolation.ElapsedMillis),
            CreatureState.WALK => PlayerMoveState.WalkTowards(playerInterpolation.Male, interpolation.Direction, interpolation.ElapsedMillis),
            CreatureState.RUN => PlayerMoveState.RunTowards(playerInterpolation.Male, interpolation.Direction, interpolation.ElapsedMillis),
            CreatureState.ATTACK => PlayerAttackState.FromInterpolation(playerInterpolation),
            CreatureState.COOLDOWN => PlayerCooldownState.Cooldown(playerInterpolation.Male, interpolation.ElapsedMillis),
            _ => throw new ArgumentOutOfRangeException()
        }
        ;
    }
}