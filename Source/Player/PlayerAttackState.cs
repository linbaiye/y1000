using System;
using y1000.Source.Creature.State;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public class PlayerAttackState : AbstractPlayerState
{
    public PlayerAttackState(SpriteManager spriteManager, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
    }
    
    public override void Update(Player c, long delta)
    {
        NotifyIfElapsed(c, delta);
    }
    
    public static PlayerAttackState Quanfa(bool male, bool below50, int millisPerSprite, long elapsedMillis = 0)
    {
        var manager = SpriteManager.LoadQuanfaAttack(male, below50, millisPerSprite);
        return new PlayerAttackState(manager, elapsedMillis);
    }

    public static PlayerAttackState FromInterpolation(PlayerInterpolation interpolation)
    {
        if (interpolation.AttackKungFuType == AttackKungFuType.FIST)
        {
            return Quanfa(interpolation.Male, interpolation.AttackKungFuBelow50, interpolation.KungFuSpriteMillis,
                interpolation.Interpolation.ElapsedMillis);
        }
        throw new NotSupportedException();
    }
}