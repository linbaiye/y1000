using y1000.Source.Creature.State;

namespace y1000.Source.Creature.Monster;

public abstract class AbstractPassiveMonster<TM> : AbstractCreature where TM : AbstractPassiveMonster<TM>
{
    private ICreatureState<TM> _state;
    
    

}