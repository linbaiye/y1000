using System;
using System.Xml.Linq;
using y1000.code;
using y1000.code.player;
using y1000.Source.Creature.State;
using y1000.Source.Networking;

namespace y1000.Source.Player;

public interface IPlayerState : ICreatureState<Player>
{
    static readonly IPlayerState Empty = EmptyPlayerState.Instance;

    static IPlayerState CreateFrom(PlayerInterpolation playerInterpolation)
    {
        var interpolation = playerInterpolation.Interpolation;
        return interpolation.State switch
        {
            CreatureState.FLY => PlayerMoveState.FlyTowards(playerInterpolation.Male, interpolation.Direction, interpolation.ElapsedMillis),
            CreatureState.WALK => PlayerMoveState.WalkTowards(playerInterpolation.Male, interpolation.Direction, interpolation.ElapsedMillis),
            _ => throw new ArgumentOutOfRangeException()
        }
        ;
    }
}