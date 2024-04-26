using Godot;
using Google.Protobuf.WellKnownTypes;
using NLog;
using y1000.code;
using y1000.code.player;
using y1000.Source.Character.State;
using y1000.Source.Sprite;

namespace y1000.Source.Creature.State;

public abstract class AbstractCreatureState<TC> : ICreatureState<TC> where TC : ICreature
{
    private readonly SpriteManager _spriteManager;

    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    protected AbstractCreatureState(SpriteManager spriteManager, long elapsedMillis = 0)
    {
        _spriteManager = spriteManager;
        ElapsedMillis = elapsedMillis;
    }
    
    protected long ElapsedMillis { get; set; }
    

    public OffsetTexture BodyOffsetTexture(TC creature)
    {
        return _spriteManager.Texture(creature.Direction, ElapsedMillis);
    }


    protected SpriteManager SpriteManager => _spriteManager;

    public abstract void Update(TC c, long delta);
}