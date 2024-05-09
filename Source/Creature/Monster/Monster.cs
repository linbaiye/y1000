using System;
using Godot;
using NLog;
using y1000.code;
using y1000.code.player;
using y1000.Source.Creature.State;
using y1000.Source.Entity;
using y1000.Source.Entity.Animation;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Creature.Monster;

public partial class Monster : AbstractCreature, IEntity, IServerMessageVisitor
{
    private ICreatureState<Monster> _state = new MonsterEmptyState();

    public override OffsetTexture BodyOffsetTexture => _state.BodyOffsetTexture(this);
    

    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

    private MonsterAnimation _animation = MonsterAnimation.Instance;
    
    private void Init(long id, Direction direction, ICreatureState<Monster> state, Vector2I coordinate, IMap map, string name)
    {
        _state = state;
        base.Init(id, direction, coordinate, map, name);
        _animation = MonsterAnimation.LoadFor(name);
    }

    public MonsterAnimation MonsterAnimation => _animation;

    private static ICreatureState<Monster> CreateState(CreatureState state, int elapses, string name, Direction direction)
    {
        switch (state)
        {
            case CreatureState.IDLE:
                return MonsterIdleState.Create(name, elapses);
            case CreatureState.WALK:
                return MonsterMoveState.MoveTowards(name, direction, elapses);
            case CreatureState.HURT:
                return MonsterHurtState.Create(name, elapses);
            default:
                throw new NotImplementedException();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        _state.Update(this, (int)(delta * 1000));
    }


    public void Visit(MoveMessage moveMessage)
    {
        _state = CreateState(CreatureState.WALK, 0, EntityName, moveMessage.Direction);
    }

    public void Visit(HurtMessage hurtMessage)
    {
        _state = CreateState(CreatureState.HURT, 0, EntityName, Direction);
    }

    public void Visit(SetPositionMessage message)
    {
        SetPosition(message);
        _state = MonsterIdleState.Create(EntityName, 0);
    }

    public void Handle(IEntityMessage message)
    {
        message.Accept(this);
    }

    public static Monster Create(CreatureInterpolation creatureInterpolation, IMap map)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/Monster.tscn");
        var monster = scene.Instantiate<Monster>();
        var interpolation = creatureInterpolation.Interpolation;
        var name = "牛";
        var state = CreateState(interpolation.State,
                interpolation.ElapsedMillis, name, interpolation.Direction);
        monster.Init(creatureInterpolation.Id, 
            interpolation.Direction, state, interpolation.Coordinate, map, name);
        if (state is AbstractCreatureMoveState<Monster> moveState)
        {
            moveState.Init(monster);
        }
        return monster;
    }


}