using y1000.Source.Creature.State;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public class PlayerAttackState : AbstractPlayerState
{
    public PlayerAttackState(SpriteManager spriteManager, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
        
    }
    
    public override void Update(Player c, long delta)
    {
        NotifyIfElapsed(c, delta);
    }
    
    public static PlayerAttackState QuanfaAttack(bool male, bool below50, int millisPerSprite, long elapsedMillis = 0)
    {
        var manager = SpriteManager.LoadQuanfaAttack(male, below50, millisPerSprite);
        return new PlayerAttackState(manager, elapsedMillis);
    }
}