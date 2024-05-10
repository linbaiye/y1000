using y1000.Source.Animation;
using y1000.Source.Entity.Animation;

namespace y1000.Source.Creature.State;

public abstract class AbstractCreatureState<TC> : ICreatureState<TC> where TC : ICreature
{
    protected AbstractCreatureState(int totalMillis, int elapsedMillis)
    {
        ElapsedMillis = elapsedMillis;
        TotalMillis = totalMillis;
    }
    
    protected int ElapsedMillis { get; set; }
    
    protected int TotalMillis { get; }

    protected void Elapse(int delta)
    {
        if (ElapsedMillis < TotalMillis)
        {
            ElapsedMillis += delta;
        }
    }

    public abstract OffsetTexture BodyOffsetTexture(TC creature);
    
    public abstract void Update(TC c, int delta);
}