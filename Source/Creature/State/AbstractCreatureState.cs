using y1000.Source.Animation;

namespace y1000.Source.Creature.State;

public abstract class AbstractCreatureState<TC> : ICreatureState<TC> where TC : ICreature
{
    protected AbstractCreatureState(int totalMillis, int elapsedMillis)
    {
        ElapsedMillis = elapsedMillis;
        TotalMillis = totalMillis;
    }
    
    public int ElapsedMillis { get; set; }
    
    public int TotalMillis { get; }

    protected bool Elapse(int delta)
    {
        if (ElapsedMillis < TotalMillis)
        {
            ElapsedMillis += delta;
        }

        return ElapsedMillis >= TotalMillis;
    }

    public abstract OffsetTexture BodyOffsetTexture(TC creature);
    
    public abstract void Update(TC c, int delta);
}