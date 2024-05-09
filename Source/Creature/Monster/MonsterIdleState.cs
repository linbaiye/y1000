using y1000.code;
using y1000.Source.Character.State;
using y1000.Source.Creature.State;
using y1000.Source.Sprite;

namespace y1000.Source.Creature.Monster;

public class MonsterIdleState : AbstractMonsterState
{
    private MonsterIdleState(MonsterAnimation animation, int elapsedMillis = 0) : base(, , elapsedMillis)
    {
    }

    public override void Update(Monster c, int delta)
    {
        if (ElapsedMillis < TotalMillis)
        {
            ElapsedMillis += delta;
        }
        else
        {
            ElapsedMillis = 0;
        }
    }

    public static MonsterIdleState Create(string name, int elapsed = 0)
    {
        return new MonsterIdleState(elapsed);
    }

    protected override CreatureState State => CreatureState.IDLE;
}