using y1000.code;
using y1000.Source.Creature.State;
using y1000.Source.Entity.Animation;

namespace y1000.Source.Creature.Monster;

public abstract class AbstractMonsterState : AbstractCreatureState<Monster>
{
    protected AbstractMonsterState(int total, int elapsed = 0) : base(total, elapsed)
    {
    }
    
    protected abstract CreatureState State { get; }

    public override OffsetTexture BodyOffsetTexture(Monster creature)
    {
        return creature.MonsterAnimation.OffsetTexture(State, creature.Direction, ElapsedMillis);
    }
}