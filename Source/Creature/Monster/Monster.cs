using System;
using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature.State;
using y1000.Source.Entity;
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

	public void AnimationDone(CreatureState state = CreatureState.FROZEN)
	{
		if (state == CreatureState.IDLE)
		{
			_state = MonsterStillState.Frozen(MonsterAnimation);
		}
		else
		{
			_state = MonsterStillState.Idle(MonsterAnimation);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		_state.Update(this, (int)(delta * 1000));
	}

	public void Visit(MoveMessage moveMessage)
	{
		_state = MonsterMoveState.Move(MonsterAnimation, moveMessage.Direction);
	}

	public void Visit(HurtMessage hurtMessage)
	{
		SetPosition(hurtMessage.Coordinate, hurtMessage.Direction);
		_state = MonsterStillState.Hurt(MonsterAnimation);
	}
	

	public void Visit(SetPositionMessage message)
	{
		SetPosition(message);
		_state = MonsterStillState.Idle(MonsterAnimation);
	}

	public void Visit(ChangeStateMessage message)
	{
		_state = CreateState(message.NewState, 0, Direction, MonsterAnimation);
	}

	public void Visit(CreatureAttackMessage attackMessage)
	{
		SetPosition(attackMessage.Coordinate, attackMessage.Direction);
		_state = MonsterStillState.Attack(MonsterAnimation);
	}

	public void Visit(RemoveEntityMessage removeEntityMessage)
	{
		LOGGER.Debug("Delete message received.");
		Delete();
	}

	public void Handle(IEntityMessage message)
	{
		//LOGGER.Debug("Recieved message {0}.", message);
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
