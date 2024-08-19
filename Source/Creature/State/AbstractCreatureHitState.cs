using y1000.code.player;
using y1000.Source.Sprite;

namespace y1000.Source.Creature.State;

public abstract class AbstractCreatureHitState<TC> : AbstractCreatureState<TC> where TC : AbstractCreature
{
    public AbstractCreatureHitState(int total, int elapsedMillis = 0) : base(total, elapsedMillis)
    {
        
    }

    public override void Update(TC c, int delta)
    {
        Elapse(delta);
    }
}