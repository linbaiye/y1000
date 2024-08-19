using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature;

namespace y1000.Source.Player;

public class PlayerDraggedState  :  IPlayerState
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    private readonly Direction _direction;
    

    public PlayerDraggedState(Direction towards)
    {
        _direction = towards;
    }
    
    public void Update(PlayerImpl player, int delta)
    {
    }
    

    public CreatureState State => CreatureState.DIE;

    public OffsetTexture BodyOffsetTexture(PlayerImpl player)
    {
        var ani = player.IsMale ? PlayerBodyAnimation.Male : PlayerBodyAnimation.Female;
        var animationMillis = PlayerBodyAnimation.Male.AnimationMillis(CreatureState.DIE);
        return ani.OffsetTexture(State, _direction, animationMillis);
    }

    public int ElapsedMillis => PlayerBodyAnimation.Male.AnimationMillis(CreatureState.DIE);
    public int TotalMillis => PlayerBodyAnimation.Male.AnimationMillis(CreatureState.DIE);
}