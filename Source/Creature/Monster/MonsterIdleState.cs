using y1000.code;
using y1000.Source.Character.State;
using y1000.Source.Creature.State;
using y1000.Source.Sprite;

namespace y1000.Source.Creature.Monster;

public class MonsterIdleState : AbstractMonsterState
{
    
    private MonsterIdleState(int total, int elapsedMillis = 0) : base(total, elapsedMillis)
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

    
    public static MonsterIdleState Create(MonsterAnimation animation, int elapsed)
    {
        return new MonsterIdleState(animation.AnimationMillis(CreatureState.IDLE), elapsed);
    }


    protected override CreatureState State => CreatureState.IDLE;
}