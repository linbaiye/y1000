using System.Collections.Generic;
using NLog;
using y1000.code;
using y1000.code.player;
using y1000.Source.Character.State;

namespace y1000.Source.Player;

public abstract class AbstractPlayerState : IPlayerState
{

    private readonly SpriteManager _spriteManager;
    
    protected AbstractPlayerState(SpriteManager spriteManager, long elapsedMillis = 0)
    {
        _spriteManager = spriteManager;
        ElapsedMillis = elapsedMillis;
    }

    protected long ElapsedMillis { get; set; }

    public OffsetTexture BodyOffsetTexture(IPlayer player)
    {
        return _spriteManager.Texture(player.Direction, ElapsedMillis);
    }

    protected SpriteManager SpriteManager => _spriteManager;
    
    public abstract void Update(Player player, long delta);
}