using y1000.code.player;

namespace y1000.Source.Creature.State;

public abstract class AbstractCreatureIdleState<TC> : ICreatureState<TC> where TC : ICreature
{

    public abstract void Update(TC c, long delta);
}