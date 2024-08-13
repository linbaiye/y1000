using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Creature.State;

namespace y1000.Source.Player;

public class PlayerDraggedState  : AbstractCreatureMoveState<PlayerImpl>, IPlayerState
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

    public PlayerDraggedState(Direction towards) : base(300, towards)
    {
    }
    
    
    protected override ILogger Logger => LOGGER;

    public override void Update(PlayerImpl player, int delta)
    {
        Move(player, delta);
        /*if (ElapsedMillis >= TotalMillis)
        {
        }*/
    }

    public override CreatureState State => CreatureState.DIE;


    public override OffsetTexture BodyOffsetTexture(PlayerImpl player)
    {
        var ani = player.IsMale ? PlayerBodyAnimation.Male : PlayerBodyAnimation.Female;
        var animationMillis = PlayerBodyAnimation.Male.AnimationMillis(CreatureState.DIE);
        return ani.OffsetTexture(CreatureState.DIE, Towards, animationMillis);
    }
}