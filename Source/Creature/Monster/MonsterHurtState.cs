using y1000.code;
using y1000.Source.Creature.State;
using y1000.Source.Sprite;

namespace y1000.Source.Creature.Monster;

public class MonsterHurtState : AbstractCreatureHitState<Monster>
{
    public MonsterHurtState(SpriteManager spriteManager, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
    }
    
    public static MonsterHurtState Create(string name, long elapsed = 0)
    {
        return new MonsterHurtState(SpriteManager.LoadForMonster(name, CreatureState.HURT), elapsed);
    }
}