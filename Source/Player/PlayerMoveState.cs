using NLog;
using y1000.code;
using y1000.Source.Creature;
using y1000.Source.Creature.State;
using y1000.Source.Entity.Animation;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public class PlayerMoveState : AbstractCreatureMoveState<Player>, IPlayerState
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    private PlayerMoveState(int total, CreatureState state, Direction towards, int elapsedMillis = 0) : base(total, towards, elapsedMillis)
    {
        State = state;
    }
    
    protected override ILogger Logger => LOGGER;

    private CreatureState State { get; set; }


    private static PlayerMoveState Create(CreatureState state, Direction dir, int e = 0)
    {
        return new PlayerMoveState(PlayerAnimation.Male.AnimationMillis(state), state, dir, e);
    }
    
    public static PlayerMoveState WalkTowards(bool male, Direction direction, int elapsed = 0)
    {
        return Create(CreatureState.WALK, direction, elapsed);
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

    public static PlayerMoveState RunTowards(bool male, Direction direction, int elapsed = 0)
    {
        return Create(CreatureState.RUN, direction, elapsed);
    }
    
    public static PlayerMoveState FlyTowards(bool male, Direction direction, int elapsed = 0)
    {
        return Create(CreatureState.RUN, direction, elapsed);
    }

}