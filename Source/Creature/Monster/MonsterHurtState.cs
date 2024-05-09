using y1000.code;
using y1000.Source.Creature.State;
using y1000.Source.Entity.Animation;
using y1000.Source.Sprite;

namespace y1000.Source.Creature.Monster;

public class MonsterHurtState : AbstractMonsterState
{
    private MonsterHurtState(int total, MonsterAnimation animation, int elapsedMillis = 0) : base(total, animation, elapsedMillis)
    {
    }
    
    public static MonsterHurtState Create(string name, long elapsed = 0)
    {
        return new MonsterHurtState(SpriteManager.LoadForMonster(name, CreatureState.HURT), elapsed);
    }

    protected override CreatureState State { get; }

    public override OffsetTexture BodyOffsetTexture(Monster creature)
    {
        return MonsterAnimation.Instance.OffsetTexture(CreatureState.HURT, creature.Direction, ElapsedMillis);
    }

    public override void Update(Monster c, int delta)
    {
    }
}