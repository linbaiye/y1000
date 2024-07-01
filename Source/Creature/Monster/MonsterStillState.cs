using y1000.Source.Animation;

namespace y1000.Source.Creature.Monster;

public sealed class MonsterStillState : AbstractMonsterState
{
    private MonsterStillState(CreatureState state, int total, int elapsed = 0) : base(total, elapsed)
    {
        State = state;
    }

    public override void Update(Monster c, int delta)
    {
        if (Elapse(delta))
        {
            c.AnimationDone(State);
        }
    }
    
    public override CreatureState State { get; }

    private static MonsterStillState Create(IAnimation animation, CreatureState st, int e = 0)
    {
        return new MonsterStillState(st, animation.AnimationMillis(st), e);
    }

    public static MonsterStillState Idle(MonsterAnimation animation, int elapsed = 0)
    {
        return Create(animation, CreatureState.IDLE, elapsed);
    }

    public static MonsterStillState Attack(MonsterAnimation animation, int elapsed = 0)
    {
        return Create(animation, CreatureState.ATTACK, elapsed);
    }
    
    public static MonsterStillState Hurt(MonsterAnimation animation, int elapsed = 0)
    {
        return Create(animation, CreatureState.HURT, elapsed);
    }
    
    public static MonsterStillState Die(MonsterAnimation animation, int elapsed = 0)
    {
        return Create(animation, CreatureState.DIE, elapsed);
    }

    public static MonsterStillState Frozen(MonsterAnimation animation, int e = 0)
    {
        return Create(animation, CreatureState.FROZEN, e);
    }
}