using System;
using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Item;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Player;

public partial class PlayerImpl: AbstractCreature, IPlayer, IServerMessageVisitor, IPlayerAnimation
{
	private IPlayerState _state = IPlayerState.Empty;

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	private PlayerWeaponAnimation? _handAnimation;

	private PlayerArmorAnimation? _chestAnimation;

	public event EventHandler<PlayerRangedAttackEventArgs>? OnPlayerShoot;
	
	public void Init(bool male, IPlayerState state, Direction direction,  Vector2I coordinate, long id, IMap map, string name)
	{
		base.Init(id, direction, coordinate, map, name);
		IsMale = male;
		_state = state;
	}
	
	
	public PlayerWeapon? Weapon { get; private set; }
	
	public PlayerChest? Chest { get; private set; }
	
	public void ChangeWeapon(PlayerWeapon weapon)
	{
		_handAnimation = PlayerWeaponAnimation.LoadFor(weapon);
		Weapon = weapon;
	}

	public void ChangeChest(PlayerChest chest)
	{
		_chestAnimation = PlayerArmorAnimation.Create(chest);
		Chest = chest;
	}

	public void Visit(PlayerChangeWeaponMessage message)
	{
		var weapon = EquipmentFactory.Instance.CreatePlayerWeapon(message.WeaponName, IsMale);
		ChangeWeapon(weapon);
		if (message.State != _state.State)
		{
			ChangeState(IPlayerState.Cooldown());
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (_state == IPlayerState.Empty)
		{
			_state = IPlayerState.Idle();
		}
	}

	public bool IsMale { get; private set; }
	
	public override OffsetTexture BodyOffsetTexture => _state.BodyOffsetTexture(this);
	
	public void ChangeState(IPlayerState newState)
	{
		_state = newState;
	}

	public void EmitRangedAttackEvent(long id)
	{
		OnPlayerShoot?.Invoke(this, new PlayerRangedAttackEventArgs(id));
	}

	public void Visit(MoveMessage message)
	{
		LOGGER.Debug("Received message {0}.", message);
		var playerState = IPlayerState.NonHurtState(message.State, message.Direction);
		ChangeState(playerState);
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		_state.Update(this, (int)(delta * 1000));
	}

	public void Visit(SetPositionMessage setPositionMessage)
	{
		SetPosition(setPositionMessage);
		if (setPositionMessage.State == CreatureState.IDLE)
		{
			ChangeState(IPlayerState.Idle());
		}
		else if (setPositionMessage.State == CreatureState.COOLDOWN)
		{
			ChangeState(IPlayerState.Cooldown());
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

	public void ResetState()
	{
		_state.Reset();
	}

	public OffsetTexture? HandTexture => _handAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? ChestTexture => _chestAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);

	public void Visit(PlayerAttackMessage message)
	{
		SetPosition(message.Coordinate, message.Direction);
		_state = IPlayerState.Attack(message);
	}

	public void Visit(HurtMessage hurtMessage)
	{
		SetPosition(hurtMessage.Coordinate, hurtMessage.Direction);
		_state = IPlayerState.Hurt(hurtMessage.AfterHurtState);
	}

	private string Location()
	{
		return $"{base.ToString()}, {nameof(Id)}: {Id}, {nameof(Direction)}: {Direction}, {nameof(Coordinate)}: {Coordinate}";
	}

	public static PlayerImpl FromInterpolation(PlayerInterpolation playerInterpolation, IMap map)
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/player.tscn");
		var player = scene.Instantiate<PlayerImpl>();
		var interpolation = playerInterpolation.Interpolation;
		var state = IPlayerState.CreateFrom(playerInterpolation);
		player.Init(playerInterpolation.Male, state, 
			interpolation.Direction, interpolation.Coordinate, playerInterpolation.Id, map, playerInterpolation.Name);
		if (state is PlayerMoveState moveState)
		{
			moveState.Init(player);
		}
		if (playerInterpolation.WeaponName != null)
		{
			player.ChangeWeapon(EquipmentFactory.Instance.CreatePlayerWeapon(playerInterpolation.WeaponName, playerInterpolation.Male));
		}

		if (playerInterpolation.ChestName != null)
		{
			player.ChangeChest(EquipmentFactory.Instance.CreatePlayerChest(playerInterpolation.ChestName, playerInterpolation.Male));
		}
		return player;
	}
}
