using System.Collections.Generic;
using Godot;
using y1000.code;
using y1000.code.player;
using y1000.Source.Character.State;

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

    private IPlayerState _state;

    public Player(bool male, IPlayerState state, Direction direction = Direction.DOWN)
    {
        IsMale = male;
        _state = state;
        Direction = direction;
    }
    
    public bool IsMale { get; }
    public Direction Direction { get; }

    public OffsetTexture BodyOffsetTexture => _state.BodyOffsetTexture(this);

    public static AnimatedSpriteManager IdleStateSpriteManager()
    {
        var container = SpriteContainer.LoadMalePlayerSprites("N02");
        return AnimatedSpriteManager.WithPinpong(500, IDLE_BODY_SPRITE_OFFSET, container);
    }
}