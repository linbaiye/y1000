using System.Collections.Generic;
using NLog;
using y1000.code;
using y1000.code.networking.message;
using y1000.Source.Character.State;

namespace y1000.Source.Player;

public class PlayerWalkState : AbstractPlayerState
{
    private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new()
    {
        { Direction.UP, 0},
        { Direction.UP_RIGHT, 6},
        { Direction.RIGHT, 12},
        { Direction.DOWN_RIGHT, 18},
        { Direction.DOWN, 24},
        { Direction.DOWN_LEFT, 30},
        { Direction.LEFT, 36},
        { Direction.UP_LEFT, 42},
    };
    
    private const int SpriteLengthMillis = 100;

    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

    private readonly Direction _towards;
    

    private bool _directionChanged;

    private PlayerWalkState(SpriteManager spriteManager, Direction towards, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
        _towards = towards;
        _directionChanged = false;
    }

    public override void Update(Player player, long delta)
    {
        if (!_directionChanged)
        {
            player.Direction = _towards;
            _directionChanged = true;
        }
        ElapsedMillis += delta;
        var animationLengthMillis = SpriteManager.AnimationLength;
        var velocity = VectorUtil.Velocity(player.Direction);
        player.Position += velocity * ((float)delta / animationLengthMillis);
        if (ElapsedMillis < animationLengthMillis)
        {
            return;
        }
        player.Position = player.Position.Snapped(VectorUtil.TILE_SIZE);
        player.ChangeState(PlayerIdleState.StartFrom(player.IsMale,0));
        LOGGER.Debug("Player {0} moved to coordinate {1}.", player.Id, player.Coordinate);
    }

    public static PlayerWalkState Towards(Direction direction, long elapsed)
    {
        var asm = SpriteManager.Normal(SpriteLengthMillis, SPRITE_OFFSET, SpriteContainer.LoadMalePlayerSprites("N02"));
        return new PlayerWalkState(asm, direction, elapsed); 
    }
}