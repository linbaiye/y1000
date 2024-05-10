using NLog;
using y1000.code;
using y1000.Source.Animation;
using y1000.Source.Creature.State;

namespace y1000.Source.Creature.Monster;

public class MonsterMoveState : AbstractCreatureMoveState<Monster>
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    private MonsterMoveState(int total, Direction towards, int elapsedMillis = 0) : base(total, towards, elapsedMillis)
    {
    }
    
    protected override ILogger Logger => LOGGER;

    public override OffsetTexture BodyOffsetTexture(Monster creature)
    {
        return creature.MonsterAnimation.OffsetTexture(CreatureState.WALK, creature.Direction, ElapsedMillis);
    }

    public override void Update(Monster c, int delta)
    {
        Move(c, delta);
    }

    public static MonsterMoveState Move(MonsterAnimation animation, Direction towards, int elapsed = 0)
    {
        return new MonsterMoveState(animation.AnimationMillis(CreatureState.WALK), towards, elapsed);
    }
}