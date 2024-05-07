using System;
using System.Collections.Generic;
using Godot;
using Google.Protobuf.WellKnownTypes;
using NLog;
using Source.Networking.Protobuf;
using y1000.code;
using y1000.code.player;
using y1000.Source.Creature;
using y1000.Source.Entity.Animation;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Player;

public partial class Player: AbstractCreature, IPlayer, IServerMessageVisitor
{

	private IPlayerState _state = IPlayerState.Empty;

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	public void Init(bool male, IPlayerState state, Direction direction,  Vector2I coordinate, long id, IMap map)
	{
		base.Init(id, direction, coordinate, map, "");
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
	
	public void ChangeState(IPlayerState newState)
	{
		_state = newState;
	}

	public void Visit(MoveMessage message)
	{
		_state = PlayerMoveState.WalkTowards(IsMale, message.Direction);
	}
	
	public void Visit(FlyMessage message)
	{
		if (_state is PlayerMoveState moveState)
		{
			moveState.CheckMoving();
		}
		_state = PlayerMoveState.FlyTowards(IsMale, message.Direction);
	}
	
	public void Visit(RunMessage message)
	{
		_state = PlayerMoveState.RunTowards(IsMale, message.Direction);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		_state.Update(this, (long)(delta * 1000));
	}

	public void Visit(SetPositionMessage setPositionMessage)
	{
		SetPosition(setPositionMessage);
		if (setPositionMessage.State == CreatureState.IDLE)
		{
			ChangeState(PlayerIdleState.StartFrom(IsMale, 0));
		}
	}


	public void Handle(IEntityMessage message)
	{
		message.Accept(this);
	}

	public void Visit(RemoveEntityMessage message)
	{
		Delete();
	}

	public void Visit(PlayerAttackMessage message)
	{
		Direction = message.Direction;
		_state = PlayerAttackState.Quanfa(IsMale, message.Below50, message.MillisPerSprite);
	}

	public void Visit(HurtMessage hurtMessage)
	{
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
		var state = IPlayerState.CreateFrom(playerInterpolation);
		player.Init(playerInterpolation.Male, state, 
			interpolation.Direction, interpolation.Coordinate, playerInterpolation.Id, map);
		return player;
	}
}
