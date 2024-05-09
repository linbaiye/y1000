using Godot;
using y1000.code.player;
using y1000.Source.Creature.State;
using y1000.Source.Entity.Animation;

namespace y1000.Source.Creature.Monster;

public class MonsterEmptyState : ICreatureState<Monster>
{
    public OffsetTexture BodyOffsetTexture(Monster player)
    {
        throw new System.NotImplementedException();
    }

    public void Update(Monster c, int delta)
    {
        throw new System.NotImplementedException();
    }
}