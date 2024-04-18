using y1000.code;
using y1000.Source.Character.State;
using y1000.Source.Creature.State;
using y1000.Source.Sprite;

namespace y1000.Source.Creature.Monster;

public class MonsterIdleState : AbstractCreatureState<Monster>
{
    private MonsterIdleState(SpriteManager spriteManager, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
    }

    public override void Update(Monster c, long delta)
    {
        if (ElapsedMillis < SpriteManager.AnimationLength)
        {
            ElapsedMillis += delta;
        }
        else
        {
            ElapsedMillis = 0;
        }
    }

    public static MonsterIdleState Create(string name, long elapsed = 0)
    {
        return new MonsterIdleState(SpriteManager.LoadForMonster(name, CreatureState.IDLE), elapsed);
    }
}