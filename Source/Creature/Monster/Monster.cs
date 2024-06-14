using System;
using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature.State;
using y1000.Source.Entity;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Util;

namespace y1000.Source.Creature.Monster;

public partial class Monster : AbstractCreature, IEntity, IServerMessageVisitor
{
	private ICreatureState<Monster> _state = new MonsterEmptyState();

	public override OffsetTexture BodyOffsetTexture => _state.BodyOffsetTexture(this);


	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	private MonsterAnimation _animation = MonsterAnimation.Instance;
	
	private void Init(long id, Direction direction, ICreatureState<Monster> state, Vector2I coordinate, IMap map, string name, MonsterAnimation animation)
	{
		base.Init(id, direction, coordinate, map, name);
		_state = state;
		_animation = animation;
	}

	public MonsterAnimation MonsterAnimation => _animation;

	private static ICreatureState<Monster> CreateState(CreatureState state, int elapses, Direction direction, MonsterAnimation animation)
	{
		switch (state)
		{
			case CreatureState.IDLE:
				return MonsterStillState.Idle(animation, elapses);
			case CreatureState.WALK:
				return MonsterMoveState.Move(animation, direction, elapses);
			case CreatureState.HURT:
				return MonsterStillState.Hurt(animation, elapses);
			case CreatureState.ATTACK:
				return MonsterStillState.Attack(animation, elapses);
			case CreatureState.FROZEN:
				return MonsterStillState.Frozen(animation, elapses);
			default:
				throw new NotImplementedException();
		}
	}

	private void ChangeState(ICreatureState<Monster> newState)
	{
		if (_state is MonsterMoveState moveState && moveState.ToCoordinate.HasValue)
		{
			if (!Position.ToCoordinate().Equals(moveState.ToCoordinate.Value))
			{
				Position = moveState.ToCoordinate.Value.ToPosition();
			}
			Map.Occupy(this);
		}
		_state = newState;
	}

	public void AnimationDone(CreatureState state = CreatureState.FROZEN)
	{
		if (state == CreatureState.IDLE)
		{
			ChangeState(MonsterStillState.Frozen(MonsterAnimation));
		}
		else
		{
			ChangeState(MonsterStillState.Idle(MonsterAnimation));
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		_state.Update(this, (int)(delta * 1000));
	}

	public void Visit(MoveMessage moveMessage)
	{
        LOGGER.Debug("Monster {2} leaving coordinate {0} for {1}.", Coordinate, Coordinate.Move(moveMessage.Direction), moveMessage.Id);
		ChangeState(MonsterMoveState.Move(MonsterAnimation, moveMessage.Direction));
	}

	public void Visit(HurtMessage hurtMessage)
	{
		ChangeState(MonsterStillState.Hurt(MonsterAnimation));
		SetPosition(hurtMessage.Coordinate, hurtMessage.Direction);
		ShowLifePercent(hurtMessage.LifePercent);
	}
	

	public void Visit(SetPositionMessage message)
	{
		ChangeState(MonsterStillState.Idle(MonsterAnimation));
		SetPosition(message);
	}

	public void Visit(ChangeStateMessage message)
	{
		ChangeState(CreateState(message.NewState, 0, Direction, MonsterAnimation));
	}

	public void Visit(CreatureAttackMessage attackMessage)
	{
		ChangeState(MonsterStillState.Attack(MonsterAnimation));
		SetPosition(attackMessage.Coordinate, attackMessage.Direction);
	}

	public void Visit(RemoveEntityMessage removeEntityMessage)
	{
		Delete();
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
		var name = creatureInterpolation.Name;
		var monsterAnimation = MonsterAnimationFactory.Instance.Load(name);
		var state = CreateState(interpolation.State,
				interpolation.ElapsedMillis, interpolation.Direction, monsterAnimation);
		monster.Init(creatureInterpolation.Id, 
			interpolation.Direction, state, interpolation.Coordinate, map, name, monsterAnimation);
		if (state is AbstractCreatureMoveState<Monster> moveState)
		{
			moveState.DriftPosition(monster);
		}
		return monster;
	}


}
