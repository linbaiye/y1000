using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Creature.State;

namespace y1000.Source.Player;

public class PlayerMoveState : AbstractCreatureMoveState<Player>, IPlayerState
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    private PlayerMoveState(int total, CreatureState state, Direction towards, int elapsedMillis = 0) : base(total, towards, elapsedMillis)
    {
        State = state;
    }
    
    protected override ILogger Logger => LOGGER;

    public CreatureState State { get; }


    private static PlayerMoveState Create(CreatureState state, Direction dir, int e = 0)
    {
        return new PlayerMoveState(PlayerAnimation.Male.AnimationMillis(state), state, dir, e);
    }

    public void CheckMoving()
    {
        if (ElapsedMillis < TotalMillis)
        {
            LOGGER.Debug("{0} millis left.",  TotalMillis - ElapsedMillis);
        }
    }

    public override OffsetTexture BodyOffsetTexture(Player player)
    {
        var ani = player.IsMale ? PlayerAnimation.Male : PlayerAnimation.Female;
        return ani.OffsetTexture(State, Towards, ElapsedMillis);
    }

    public override void Update(Player player, int delta)
    {
        Move(player, delta);
        if (ElapsedMillis >= TotalMillis)
        {
            player.NotifyAnimationFinished(new CreatureAnimationDoneEventArgs(this));
        }
    }

    public static PlayerMoveState RunTowards(Direction direction, int elapsed = 0)
    {
        return Create(CreatureState.RUN, direction, elapsed);
    }

    public static PlayerMoveState EnfightWalk(Direction direction, int elapsed = 0)
    {
        return Create(CreatureState.ENFIGHT_WALK, direction, elapsed);
    }
    
    public static PlayerMoveState FlyTowards(Direction direction, int elapsed = 0)
    {
        return Create(CreatureState.FLY, direction, elapsed);
    }
    
    public static PlayerMoveState WalkTowards(Direction direction, int elapsed = 0)
    {
        return Create(CreatureState.WALK, direction, elapsed);
    }

}