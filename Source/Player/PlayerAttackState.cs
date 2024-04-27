using y1000.code;
using y1000.Source.Creature.State;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public class PlayerAttackState : AbstractCreatureAttackState<Player>, IPlayerState
{
    public PlayerAttackState(SpriteManager spriteManager, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
        
    }

    public override void Update(Player c, long delta)
    {
        ElapsedMillis += delta;
        if (ElapsedMillis >= SpriteManager.AnimationLength)
        {
            c.NotifyAnimationFinished();
        }
    }

    public static PlayerAttackState Create(bool male, bool below50, long elapsedMillis = 0)
    {
        var manager = SpriteManager.LoadFistAttackForMale(male, below50);
        return new PlayerAttackState(manager, elapsedMillis);
    }
}