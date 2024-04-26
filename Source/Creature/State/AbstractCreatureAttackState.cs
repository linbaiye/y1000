using y1000.code.player;
using y1000.Source.Sprite;

namespace y1000.Source.Creature.State;

public abstract class AbstractCreatureAttackState<TC> : AbstractCreatureState<TC> where TC : ICreature
{
    protected AbstractCreatureAttackState(SpriteManager spriteManager, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
    }
}