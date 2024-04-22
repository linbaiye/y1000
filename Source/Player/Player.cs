using System;
using System.Collections.Generic;
using Godot;
using NLog;
using Source.Networking.Protobuf;
using y1000.code;
using y1000.code.networking.message;
using y1000.code.player;
using y1000.Source.Character.State;
using y1000.Source.Creature;
using y1000.Source.Creature.State;
using y1000.Source.Map;
using y1000.Source.Networking;

namespace y1000.Source.Player;

public partial class Player: AbstractCreature, IPlayer
{

	private IPlayerState _state = IPlayerState.Empty;

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
	public event EventHandler<CreatureAnimationDoneEventArgs>? StateAnimationEventHandler;

	public void Init(bool male, IPlayerState state, Direction direction,  Vector2I coordinate, long id, IMap map)
	{
		base.Init(id, direction, coordinate, map);
		IsMale = male;
		_state = state;
	}

	public override void _Ready()
	{
		if (_state == IPlayerState.Empty)
		{
			_state = PlayerIdleState.StartFrom(true, 0);
		}
	}

	public bool IsMale { get; private set; }
	
	public override OffsetTexture BodyOffsetTexture => _state.BodyOffsetTexture(this);
	
	private static IPlayerState CreateState(bool male, CreatureState state, long start, Direction direction)
	{
		switch (state)
		{
			case CreatureState.IDLE:
				return PlayerIdleState.StartFrom(male, start);
			case CreatureState.WALK:
				return PlayerMoveState.WalkTowards(male, direction, start);
			case CreatureState.RUN:
				return PlayerMoveState.RunTowards(male, direction, start);
			case CreatureState.FLY:
				return PlayerMoveState.FlyTowards(male, direction, start);
			default:
				throw new NotSupportedException();
		}
	}

	private void Turn(AbstractPositionMessage turnMessage)
	{
		Direction = turnMessage.Direction;
		_state = PlayerIdleState.StartFrom(IsMale, 0);
	}

	public void ChangeState(IPlayerState newState)
	{
		_state = newState;
	}

	private void Move(MoveMessage message)
	{
		_state = PlayerMoveState.WalkTowards(IsMale, message.Direction);
	}
	
	private void Fly(FlyMessage message)
	{
		_state = PlayerMoveState.FlyTowards(IsMale, message.Direction);
	}
	
	private void Run(RunMessage message)
	{
		_state = PlayerMoveState.RunTowards(IsMale, message.Direction);
	}

	public override void _PhysicsProcess(double delta)
	{
		_state.Update(this, (long)(delta * 1000));
	}


	public void NotifyAnimationFinished()
	{
		StateAnimationEventHandler?.Invoke(this, new CreatureAnimationDoneEventArgs(_state));
	}

	public void Handle(IEntityMessage message)
	{
		switch (message)
		{
			case TurnMessage turnMessage:
				Turn(turnMessage);
				break;
			case MoveMessage moveMessage:
				Move(moveMessage);
				break;
			case SetPositionMessage positionMessage:
				SetPosition(positionMessage);
				break;
			case FlyMessage flyMessage:
				Fly(flyMessage);
				break;
			case RunMessage runMessage:
				Run(runMessage);
				break;
		}
	}

	public string Location()
	{
		return $"{base.ToString()}, {nameof(Id)}: {Id}, {nameof(Direction)}: {Direction}, {nameof(Coordinate)}: {Coordinate}";
	}

	public static Player FromInterpolation(PlayerInterpolation playerInterpolation, IMap map)
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/player.tscn");
		var player = scene.Instantiate<Player>();
		var interpolation = playerInterpolation.Interpolation;
		var state = CreateState(playerInterpolation.Male, interpolation.State,
			interpolation.ElapsedMillis,
			interpolation.Direction);
		player.Init(playerInterpolation.Male, state, 
			interpolation.Direction, interpolation.Coordinate, playerInterpolation.Id, map);
		return player;
	}
}
