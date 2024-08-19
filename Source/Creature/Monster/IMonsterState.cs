using y1000.Source.Animation;
using y1000.Source.Creature.State;

namespace y1000.Source.Creature.Monster;

public interface IMonsterState : ICreatureState<Monster>
{
    OffsetTexture? EffectOffsetTexture(Monster creature);
}