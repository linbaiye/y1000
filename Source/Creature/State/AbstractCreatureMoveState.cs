using Godot;
using NLog;
using y1000.Source.Util;

namespace y1000.Source.Creature.State;

public abstract class AbstractCreatureMoveState<TC> : AbstractCreatureState<TC> where TC : AbstractCreature
{
    private bool _directionChanged;
    
    private readonly Vector2 _velocity;

    protected AbstractCreatureMoveState(int total, Direction towards, int elapsedMillis = 0) : base(total, elapsedMillis)
    {
        Towards = towards;
        _directionChanged = false;
        _velocity = VectorUtil.Velocity(towards);
        ElapsedMillis = 0;
    }
    
    protected Direction Towards { get; }
    
    protected abstract ILogger Logger { get; }

    public void DriftPosition(TC creature)
    {
        creature.Position = (_velocity * ((float)ElapsedMillis / TotalMillis)) + creature.Coordinate.ToPosition();
    }

    protected void Move(TC creature, int delta)
    {
        var animationLengthMillis = TotalMillis;
        if (ElapsedMillis >= animationLengthMillis)
        {
            return;
        }
        if (!_directionChanged)
        {
            creature.Direction = Towards;
            _directionChanged = true;
        }
        ElapsedMillis += delta;
        if (ElapsedMillis > animationLengthMillis)
        {
            creature.Position += _velocity * ((float)(ElapsedMillis - animationLengthMillis) / animationLengthMillis);
        }
        else
        {
            creature.Position += _velocity * ((float)delta / animationLengthMillis);
        }
        if (ElapsedMillis >= animationLengthMillis)
        {
            creature.Position = creature.Position.Snapped(VectorUtil.TileSize);
            creature.Map.Occupy(creature);
        }
    }
}