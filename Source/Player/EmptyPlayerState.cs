using System;
using Godot;
using y1000.code.player;
using y1000.Source.Animation;
using y1000.Source.Creature;

namespace y1000.Source.Player;

public class EmptyPlayerState : IPlayerState
{
    public static readonly EmptyPlayerState Instance = new EmptyPlayerState();
    private EmptyPlayerState (){}
    public OffsetTexture BodyOffsetTexture(PlayerImpl player)
    {
        throw new System.NotImplementedException();
    }

    public void Update(PlayerImpl c, int delta)
    {
        throw new System.NotImplementedException();
    }

    public CreatureState State => throw new NotImplementedException();
    public int ElapsedMillis { get; }
}