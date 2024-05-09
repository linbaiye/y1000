using y1000.code;
using y1000.Source.Creature.State;
using y1000.Source.Entity.Animation;
using y1000.Source.Sprite;

namespace y1000.Source.Creature.Monster;

public class MonsterHurtState : AbstractMonsterState
{
    private MonsterHurtState(int total, int elapsedMillis = 0) : base(total, elapsedMillis)
    {
    }
    
    public static MonsterHurtState Create(string name, int elapsed = 0)
    {
        return null;
    }

    public static MonsterHurtState Create(MonsterAnimation animation, int elapsed)
    {
        return new MonsterHurtState(animation.AnimationMillis(CreatureState.HURT), elapsed);
    }

    protected override CreatureState State => CreatureState.HURT;

    public override void Update(Monster c, int delta)
    {
    }
}