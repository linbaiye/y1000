using NLog;
using y1000.code;
using y1000.Source.Character.State;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public class PlayerFlyState : AbstractPlayerMoveState
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    private PlayerFlyState(SpriteManager spriteManager, Direction towards, long elapsedMillis = 0) : base(spriteManager, towards, elapsedMillis)
    {
    }
    
    public static PlayerFlyState Towards(bool male, Direction direction, long elapsed = 0L)
    {
        return new PlayerFlyState(SpriteManager.LoadForPlayer(male, CreatureState.FLY), direction, elapsed); 
    }

    protected override ILogger Logger => LOGGER;
}