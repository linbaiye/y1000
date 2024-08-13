using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Creature.State;

namespace y1000.Source.Player;

public class PlayerMoveState : AbstractCreatureMoveState<PlayerImpl>, IPlayerState
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

    private PlayerMoveState(int total, CreatureState state, Direction towards, int elapsedMillis = 0) : base(total, towards, elapsedMillis)
    {
        State = state;
    }
    
    public PlayerMoveState(CreatureState state, Direction towards, int elapsedMillis = 0) : this(PlayerBodyAnimation.Male.AnimationMillis(state), state, towards, elapsedMillis)
    {
    }
    
    
    protected override ILogger Logger => LOGGER;

    public override CreatureState State { get; }


    private static PlayerMoveState Create(CreatureState state, Direction dir, int e = 0)
    {
        return new PlayerMoveState(PlayerBodyAnimation.Male.AnimationMillis(state), state, dir, e);
    }


    public override OffsetTexture BodyOffsetTexture(PlayerImpl player)
    {
        var ani = player.IsMale ? PlayerBodyAnimation.Male : PlayerBodyAnimation.Female;
        return ani.OffsetTexture(State, Towards, ElapsedMillis);
    }

    public override void Update(PlayerImpl player, int delta)
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