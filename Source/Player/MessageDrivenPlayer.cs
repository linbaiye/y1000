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
    private readonly Player _player;
    
    private readonly Queue<IEntityMessage> _messages;
    public MessageDrivenPlayer(Player player)
    {
        player.StateAnimationEventHandler += OnStateAnimationDone;
        _player = player;
        _messages = new Queue<IEntityMessage>();
    }

    public Player Player => _player;
    public string EntityName => _player.EntityName;
    public long Id => _player.Id;
    public Vector2I Coordinate => _player.Coordinate;

    private void OnStateAnimationDone(object? sender, CreatureAnimationDoneEventArgs args)
    {
        switch (args.FinishedState.State)
        {
            case CreatureState.ATTACK:
                _player.ChangeState(IPlayerState.Cooldown());
                break;
            case CreatureState.IDLE:
                _player.ChangeState(IPlayerState.Idle());
                break;
        }
    }

    public void Handle(IEntityMessage message)
    {
        _player.Handle(message);
    }

    public static MessageDrivenPlayer FromInterpolation(PlayerInterpolation playerInterpolation, IMap map)
    {
        var player = Player.FromInterpolation(playerInterpolation, map);
        return new MessageDrivenPlayer(player);
    }
}