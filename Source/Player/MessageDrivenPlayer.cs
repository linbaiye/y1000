using System.Collections.Generic;
using Godot;
using y1000.Source.Animation;
using y1000.Source.Character;
using y1000.Source.Creature;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Util;

namespace y1000.Source.Player;

public class MessageDrivenPlayer : IPlayer
{
    private readonly Queue<IEntityMessage> _messages;

    private MessageDrivenPlayer(PlayerImpl player)
    {
        Player = player;
        Player.StateAnimationEventHandler += OnStateAnimationDone;
        _messages = new Queue<IEntityMessage>();
    }

    public PlayerImpl Player { get; }

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
                Player.ChangeState(IPlayerState.NonHurtState(((PlayerHurtState)args.FinishedState).AfterHurt));
                break;
            case CreatureState.DIE:
            case CreatureState.SIT:
            case CreatureState.FROZEN:
                break;
        }
    }

    public void Handle(IEntityMessage message)
    {
        Player.Handle(message);
    }

    public bool HasPoint(Vector2 point)
    {
        return Player.BodyRectangle.HasPoint(point);
    }

    public bool CanBeTraded(CharacterImpl character)
    {
        return Player.Coordinate.Distance(character.Coordinate) <= 2;
    }

    public bool Dead => Player.Dead;

    public static MessageDrivenPlayer FromInterpolation(PlayerInterpolation playerInterpolation,
        IMap map)
    {
        var player = PlayerImpl.FromInterpolation(playerInterpolation, map);
        return new MessageDrivenPlayer(player);
    }

    public OffsetTexture BodyOffsetTexture => Player.BodyOffsetTexture;
    public Vector2 OffsetBodyPosition => Player.OffsetBodyPosition;
    public Direction Direction => Player.Direction;
    public Rect2 BodyRectangle => Player.BodyRectangle;
}