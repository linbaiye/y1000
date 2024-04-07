using y1000.code.player;
using y1000.Source.Character.State;

namespace y1000.Source.Player;

public class PlayerIdleState  : AbstractPlayerState
{
    public PlayerIdleState(AnimatedSpriteManager spriteManager, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
    }
    
    public override void Update(long delta)
    {
        ElapsedMillis += delta;
    }
}