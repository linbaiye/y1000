using Godot;
using NLog;
using y1000.code;
using y1000.Source.Sprite;

namespace y1000.Source.Creature.State;

public abstract class AbstractCreatureMoveState<TC> : AbstractCreatureState<TC> where TC : AbstractCreature
{
    private bool _directionChanged;
    
    private readonly Vector2 _velocity;

    protected AbstractCreatureMoveState(SpriteManager spriteManager, Direction towards, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
        Towards = towards;
        _directionChanged = false;
        _velocity = VectorUtil.Velocity(towards);
    }
    
    private Direction Towards { get; }
    
    protected abstract ILogger Logger { get; }
    

    protected void Elapse(TC creature, long delta)
    {
        if (!_directionChanged)
        {
            creature.Direction = Towards;
            _directionChanged = true;
        }
        var animationLengthMillis = SpriteManager.AnimationLength;
        if (ElapsedMillis < animationLengthMillis)
        {
            ElapsedMillis += delta;
            creature.Position += _velocity * ((float)delta / animationLengthMillis);
        }
        if (ElapsedMillis >= animationLengthMillis)
        {
            creature.Position = creature.Position.Snapped(VectorUtil.TileSize);
            Logger.Debug("Player {0} moved to coordinate {1}.", creature.Id, creature.Coordinate);
        }
    }
}