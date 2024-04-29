using Google.Protobuf.WellKnownTypes;
using y1000.Source.Entity;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public class PlayerCooldownState : AbstractPlayerState
{
    public PlayerCooldownState(SpriteManager spriteManager,
        long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
    }

    public override void Update(Player c, long delta)
    {
        NotifyIfElapsed(c, delta);
    }

    public static PlayerCooldownState Cooldown(bool male, int millisPerSprite)
    {
        SpriteManager spriteManager = SpriteManager.LoadPlayerCooldown(male, millisPerSprite);
        return new PlayerCooldownState(spriteManager);
    }
}