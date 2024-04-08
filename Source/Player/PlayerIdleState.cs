using System.Collections.Generic;
using y1000.code;
using y1000.code.player;
using y1000.Source.Character.State;
using y1000.Source.Networking;

namespace y1000.Source.Player;

public class PlayerIdleState  : AbstractPlayerState
{
    private static readonly Dictionary<Direction, int> BODY_SPRITE_OFFSET = new()
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

    private PlayerIdleState(AnimatedSpriteManager spriteManager, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
    }

    public override void Update(Player player, long delta)
    {
        ElapsedMillis += delta;
    }

    public static PlayerIdleState StartFrom(bool male, long elapsedMillis)
    {
        SpriteContainer spriteContainer =
            male ? SpriteContainer.LoadMalePlayerSprites("N02") : SpriteContainer.EmptyContainer;
        var animatedSpriteManager = AnimatedSpriteManager.WithPinpong(500, BODY_SPRITE_OFFSET, spriteContainer);
        return new PlayerIdleState(animatedSpriteManager, elapsedMillis);
    }
}