using System.Collections.Generic;
using y1000.code;
using y1000.Source.Character.State;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public class PlayerIdleState  : AbstractPlayerState
{
    private PlayerIdleState(SpriteManager spriteManager, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
    }

    public override void Update(Player player, long delta)
    {
        NotifyIfElapsed(player, delta);
    }

    public static PlayerIdleState StartFrom(bool male, long elapsedMillis)
    {
        return new PlayerIdleState(SpriteManager.LoadForPlayer(male, CreatureState.IDLE), elapsedMillis);
    }
}