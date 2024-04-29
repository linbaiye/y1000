using y1000.code.player;
using y1000.Source.Sprite;

namespace y1000.Source.Creature.State;

public abstract class AbstractCreatureHitState<TC> : AbstractCreatureState<TC> where TC : AbstractCreature
{
    public AbstractCreatureHitState(SpriteManager spriteManager, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
        
    }

    public override void Update(TC c, long delta)
    {
        Elapse(delta);
    }
}