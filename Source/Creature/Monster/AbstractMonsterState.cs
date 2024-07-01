using y1000.code;
using y1000.Source.Animation;
using y1000.Source.Creature.State;

namespace y1000.Source.Creature.Monster;

public abstract class AbstractMonsterState : AbstractCreatureState<Monster>
{
    protected AbstractMonsterState(int total, int elapsed = 0) : base(total, elapsed)
    {
    }
    
    public override OffsetTexture BodyOffsetTexture(Monster creature)
    {
        return creature.MonsterAnimation.OffsetTexture(State, creature.Direction, ElapsedMillis);
    }
}