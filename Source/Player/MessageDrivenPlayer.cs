using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
using y1000.Source.Creature;
using y1000.Source.Entity;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Player;

public class MessageDrivenPlayer : IEntity
{
    private readonly Queue<IEntityMessage> _messages;

    private MessageDrivenPlayer(Player player)
    {
        Player = player;
        Player.StateAnimationEventHandler += OnStateAnimationDone;
        _messages = new Queue<IEntityMessage>();
    }

    public Player Player { get; }

    public string EntityName => Player.EntityName;
    public long Id => Player.Id;
    public Vector2I Coordinate => Player.Coordinate;

    private void OnStateAnimationDone(object? sender, CreatureAnimationDoneEventArgs args)
    {
        switch (args.FinishedState.State)
        {
            case CreatureState.IDLE:
            case CreatureState.COOLDOWN:
                Player.ResetState();
                break;
            case CreatureState.STANDUP:
            case CreatureState.HELLO:
            case CreatureState.FLY:
            case CreatureState.WALK:
            case CreatureState.RUN:
                Player.ChangeState(IPlayerState.Idle());
                break;
            case CreatureState.AXE:
            case CreatureState.BOW:
            case CreatureState.FIST:
            case CreatureState.KICK:
            case CreatureState.SWORD:
            case CreatureState.SWORD2H:
            case CreatureState.BLADE:
            case CreatureState.BLADE2H:
            case CreatureState.SPEAR:
            case CreatureState.ENFIGHT_WALK:
            case CreatureState.THROW:
                Player.ChangeState(IPlayerState.Cooldown());
                break;
            case CreatureState.HURT:
                Player.ChangeState(args.FinishedState is PlayerStillState ?
                        IPlayerState.Idle()
                    : ((PlayerHurtState)args.FinishedState).InterruptedState);
                break;
            case CreatureState.DIE:
            case CreatureState.SIT:
            case CreatureState.TURN:
                break;
        }
    }

    public void Handle(IEntityMessage message)
    {
        Player.Handle(message);
    }

    public static MessageDrivenPlayer FromInterpolation(PlayerInterpolation playerInterpolation, IMap map)
    {
        var player = Player.FromInterpolation(playerInterpolation, map);
        return new MessageDrivenPlayer(player);
    }
}