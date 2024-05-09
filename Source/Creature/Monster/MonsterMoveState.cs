using NLog;
using y1000.code;
using y1000.Source.Creature.State;
using y1000.Source.Entity.Animation;

namespace y1000.Source.Creature.Monster;

public class MonsterMoveState : AbstractCreatureMoveState<Monster>
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    private MonsterMoveState(Direction towards, int elapsedMillis = 0) : base(MonsterAnimation.Instance.AnimationMillis(CreatureState.WALK), towards, elapsedMillis)
    {
    }
    
    protected override ILogger Logger => LOGGER;

    public override OffsetTexture BodyOffsetTexture(Monster creature)
    {
        return MonsterAnimation.Instance
    }

    public override void Update(Monster c, int delta)
    {
        Move(c, delta);
    }

    public static MonsterMoveState MoveTowards(string name, Direction towards, int elapsed = 0)
    {
        return new MonsterMoveState(towards, elapsed);
    }

}