using NLog;
using y1000.code;
using y1000.Source.Creature.State;
using y1000.Source.Sprite;

namespace y1000.Source.Creature.Monster;

public class MonsterMoveState : AbstractCreatureMoveState<Monster>
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    public MonsterMoveState(SpriteManager spriteManager, Direction towards, long elapsedMillis = 0) : base(spriteManager, towards, elapsedMillis)
    {
    }
    
    protected override ILogger Logger => LOGGER;
    
    public override void Update(Monster c, long delta)
    {
        Elapse(c, delta);
        if (ElapsedMillis >= SpriteManager.AnimationLength)
        {
            
        }
    }

    public static MonsterMoveState MoveTowards(string name, Direction towards, long elapsed = 0)
    {
        return new MonsterMoveState(SpriteManager.LoadForMonster(name, CreatureState.WALK), towards, elapsed);
    }


}