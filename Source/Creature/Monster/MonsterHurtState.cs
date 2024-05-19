using y1000.code;
using y1000.Source.Animation;
using y1000.Source.Creature.State;

namespace y1000.Source.Creature.Monster;

public class MonsterHurtState : AbstractCreatureHurtState<Monster>
{
    private MonsterHurtState(CreatureState afterHurt, int total, int elapsedMillis = 0) : base(total, elapsedMillis, afterHurt)
    {
    }

    public static MonsterHurtState Create(MonsterAnimation animation, CreatureState after, int elapsed = 0)
    {
        return new MonsterHurtState(after, animation.AnimationMillis(CreatureState.HURT), elapsed);
    }

    public override OffsetTexture BodyOffsetTexture(Monster creature)
    {
        return creature.MonsterAnimation.OffsetTexture(CreatureState.HURT, creature.Direction, ElapsedMillis);
    }
    
    public override void Update(Monster c, int delta)
    {
        if (Elapse(delta))
        {
            c.AnimationDone();
        }
    }
}