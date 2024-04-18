using y1000.code.player;
using y1000.Source.Creature.State;

namespace y1000.Source.Creature.Monster;

public class MonsterEmptyState : ICreatureState<Monster>
{
    public OffsetTexture BodyOffsetTexture(Monster c)
    {
        throw new System.NotImplementedException();
    }

    public void Update(Monster c, long delta)
    {
        throw new System.NotImplementedException();
    }
}