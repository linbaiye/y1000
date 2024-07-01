using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature.State;

namespace y1000.Source.Creature.Monster;

public sealed class MonsterMoveState : AbstractCreatureMoveState<Monster>
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

    private readonly int _speedRate;

    private MonsterMoveState(int total, Direction towards, int elapsedMillis = 0, int speedRate = 1) : base(total, towards, elapsedMillis)
    {
        _speedRate = speedRate;
    }
    
    protected override ILogger Logger => LOGGER;

    public override OffsetTexture BodyOffsetTexture(Monster creature)
    {
        var elapsed = ElapsedMillis * _speedRate;
        if (elapsed > TotalMillis)
        {
            elapsed = TotalMillis;
        }
        return creature.MonsterAnimation.OffsetTexture(CreatureState.WALK, creature.Direction, elapsed);
    }

    public override void Update(Monster c, int delta)
    {
        Move(c, delta);
    }

    public override CreatureState State => CreatureState.WALK;

    public static MonsterMoveState Move(MonsterAnimation animation, Direction towards, int speed, int elapsed = 0)
    {
        var originSpeed= animation.AnimationMillis(CreatureState.WALK);
        int rate = originSpeed / speed;
        return new MonsterMoveState(speed, towards, elapsed, rate);
    }
}