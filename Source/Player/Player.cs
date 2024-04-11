using System;
using System.Collections.Generic;
using Godot;
using NLog;
using Source.Networking.Protobuf;
using y1000.code;
using y1000.code.networking.message;
using y1000.code.player;
using y1000.Source.Character.State;
using y1000.Source.Networking;

namespace y1000.Source.Player;

public partial class Player: Node2D, IPlayer, IBody
{

	private IPlayerState _state = IPlayerState.Empty;

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	public event EventHandler? PlayerEventHandler;

	public void Init(bool male, IPlayerState state, Direction direction,  Vector2I coordinate, long id)
	{
		Id = id;
		IsMale = male;
		_state = state;
		Direction = direction;
		Position = coordinate.ToPosition();
	}

	public override void _Ready()
	{
		if (_state == IPlayerState.Empty)
		{
			_state = PlayerIdleState.StartFrom(true, 0);
		}
	}

	public bool IsMale { get; private set; }
	
	public long Id { get; private set; }

	public Direction Direction { get; set; }
	
	public OffsetTexture BodyOffsetTexture => _state.BodyOffsetTexture(this);
	
	public Vector2I Coordinate => Position.ToCoordinate();

	private static IPlayerState CreateState(bool male, CreatureState state, long start)
	{
		switch (state)
		{
			case CreatureState.IDLE:
				return PlayerIdleState.StartFrom(male, start);
			case CreatureState.WALK:
			
			default:
				throw new NotSupportedException();
		}
	}

	public void EmitEvent(PlayerMovedEvent @event)
	{
		PlayerEventHandler?.Invoke(this, new PlayerEventArgs(@event));
	}

	private void Turn(AbstractPositionMessage turnMessage)
	{
		Direction = turnMessage.Direction;
		_state = PlayerIdleState.StartFrom(IsMale, 0);
	}

	private void SetPosition(AbstractPositionMessage message)
	{
		Position = message.Coordinate.ToPosition();
		Turn(message);
	}

	public void ChangeState(IPlayerState newState)
	{
		_state = newState;
	}

	private void Move(MoveMessage message)
	{
		_state = PlayerWalkState.Towards(message.Direction, 0);
	}

	public override void _Process(double delta)
	{
		_state.Update(this, (long)(delta * 1000));
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
		}
	}

	public string Location()
	{
		return $"{base.ToString()}, {nameof(Id)}: {Id}, {nameof(Direction)}: {Direction}, {nameof(Coordinate)}: {Coordinate}";
	}

	public static Player FromInterpolation(PlayerInterpolation playerInterpolation)
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/player.tscn");
		var player = scene.Instantiate<Player>();
		var state = CreateState(playerInterpolation.Male, playerInterpolation.Interpolation.State, playerInterpolation.Interpolation.ElapsedMillis);
		player.Init(playerInterpolation.Male, state, 
			playerInterpolation.Interpolation.Direction, playerInterpolation.Interpolation.Coordinate, playerInterpolation.Id);
		return player;
	}

	public static Player Load(long id, Vector2I coordinate, bool male)
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/player.tscn");
		var player = scene.Instantiate<Player>();
		player.Init(male, PlayerIdleState.StartFrom(male, 0), Direction.DOWN, coordinate, id);
		return player;
	}

	public string EntityName => "";
}
