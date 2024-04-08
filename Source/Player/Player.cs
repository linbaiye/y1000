using System;
using System.Collections.Generic;
using Godot;
using Source.Networking.Protobuf;
using y1000.code;
using y1000.code.player;
using y1000.Source.Character.State;
using y1000.Source.Networking;

namespace y1000.Source.Player;

public partial class Player: Node2D, IPlayer
{
    private static readonly Dictionary<Direction, int> IDLE_BODY_SPRITE_OFFSET = new()
    {
        { Direction.UP, 48},
        { Direction.UP_RIGHT, 51},
        { Direction.RIGHT, 54},
        { Direction.DOWN_RIGHT, 57},
        { Direction.DOWN, 60},
        { Direction.DOWN_LEFT, 63},
        { Direction.LEFT, 66},
        { Direction.UP_LEFT, 69},
    };

    private IPlayerState _state = IPlayerState.Empty;

    private void Init(bool male, IPlayerState state, Direction direction,  Vector2I coordinate)
    {
        IsMale = male;
        _state = state;
        Direction = direction;
        Position = coordinate.ToPosition();
        ZIndex = 3;
        Visible = true;
    }

    public bool IsMale { get; private set; }

    public Direction Direction { get; set; }

    public OffsetTexture BodyOffsetTexture => _state.BodyOffsetTexture(this);

    public static AnimatedSpriteManager IdleStateSpriteManager()
    {
        var container = SpriteContainer.LoadMalePlayerSprites("N02");
        return AnimatedSpriteManager.WithPinpong(500, IDLE_BODY_SPRITE_OFFSET, container);
    }


    private static IPlayerState CreateState(bool male, CreatureState state, long start)
    {
        switch (state)
        {
            case CreatureState.IDLE:
                return PlayerIdleState.StartFrom(male, start);
            case CreatureState.WALK:
            
            default:
                throw new NotSupportedException();
        }
        ;
    }

    public static Player FromInterpolation(PlayerInterpolation playerInterpolation)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/player.tscn");
        var player = scene.Instantiate<Player>();
        var state = CreateState(playerInterpolation.Male, playerInterpolation.Interpolation.State, playerInterpolation.Interpolation.ElapsedMillis);
        player.Init(playerInterpolation.Male, state, 
            playerInterpolation.Interpolation.Direction, playerInterpolation.Interpolation.Coordinate);
        return player;
    }
}