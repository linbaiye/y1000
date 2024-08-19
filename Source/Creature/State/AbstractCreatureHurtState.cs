using y1000.Source.Animation;

namespace y1000.Source.Creature.State;

public abstract class AbstractCreatureHurtState<TC> : AbstractCreatureState<TC> where TC: ICreature
{
    protected AbstractCreatureHurtState(int totalMillis, int elapsedMillis, CreatureState afterHurtState) : base(totalMillis, elapsedMillis)
    {
        AfterHurtState = afterHurtState;
    }
    
    protected CreatureState AfterHurtState { get; }
    
}