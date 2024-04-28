using Google.Protobuf.WellKnownTypes;
using y1000.Source.Entity;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public class PlayerAttackCooldownState : AbstractPlayerState
{
    private readonly IEntity _target;
    
    public PlayerAttackCooldownState(SpriteManager spriteManager, IEntity target, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
        _target = target;
    }

    public override void Update(Player c, long delta)
    {
    }
    
}