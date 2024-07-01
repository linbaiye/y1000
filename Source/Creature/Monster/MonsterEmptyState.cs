using Godot;
using y1000.code.player;
using y1000.Source.Animation;
using y1000.Source.Creature.State;

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

    public CreatureState State => CreatureState.AXE;
}