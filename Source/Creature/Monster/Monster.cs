using System;
using Godot;
using y1000.code;
using y1000.code.entity;
using y1000.code.networking.message;
using y1000.code.player;
using y1000.Source.Creature.State;
using y1000.Source.Networking;

namespace y1000.Source.Creature.Monster;

public partial class Monster : AbstractCreature, IEntity
{
    private ICreatureState<Monster> _state = new MonsterEmptyState();

    public override OffsetTexture BodyOffsetTexture => _state.BodyOffsetTexture(this);
    
    private void Init(long id, Direction direction, ICreatureState<Monster> state, Vector2I coordinate)
    {
        _state = state;
        Direction = direction;
        Id = id;
        Position = coordinate.ToPosition();
    }

    private static ICreatureState<Monster> CreateState(CreatureState state, long elapses, string name, Direction direction)
    {
        switch (state)
        {
            case CreatureState.IDLE:
                return MonsterIdleState.Create(name, elapses);
            case CreatureState.WALK:
                return MonsterMoveState.MoveTowards(name, direction, elapses);
            default:
                throw new NotImplementedException();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        _state.Update(this, (long)(delta * 1000));
    }


    public void Handle(IEntityMessage message)
    {
        switch (message)
        {
            case MoveMessage moveMessage:
                _state = CreateState(CreatureState.WALK, 0, "牛", moveMessage.Direction);
                break;
            case SetPositionMessage positionMessage:
                SetPosition(positionMessage);
                _state = MonsterIdleState.Create( "牛", 0);
                break;
        }
    }

    public static Monster Create(CreatureInterpolation creatureInterpolation)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/Monster.tscn");
        var monster = scene.Instantiate<Monster>();
        var interpolation = creatureInterpolation.Interpolation;
        var state = CreateState(interpolation.State,
                interpolation.ElapsedMillis, "buffalo", interpolation.Direction);
        monster.Init(creatureInterpolation.Id, 
            interpolation.Direction, state, interpolation.Coordinate);
        return monster;
    }
}