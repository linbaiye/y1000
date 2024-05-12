using NLog;
using y1000.code;
using y1000.Source.Animation;

namespace y1000.Source.Creature.Monster;

public class MonsterAttackState : AbstractMonsterState
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    public MonsterAttackState(int total, int elapsed = 0) : base(total, elapsed)
    {
    }

    public override void Update(Monster c, int delta)
    {
        Elapse(delta);
        if (ElapsedMillis >= TotalMillis)
        {
            c.GoIdle();
        }
    }

    protected override CreatureState State => CreatureState.ATTACK;


    public static MonsterAttackState Create(MonsterAnimation animation, int e = 0)
    {
        Logger.Debug("Create attack");
        return new MonsterAttackState(animation.AnimationMillis(CreatureState.ATTACK), e);
    }
}