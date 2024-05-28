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
	
	private PlayerArmorAnimation? _hairAnimation;
	
	private PlayerArmorAnimation? _hatAnimation;
	
	private PlayerArmorAnimation? _wristAnimation;
	
	private PlayerArmorAnimation? _bootAnimation;

	public event EventHandler<PlayerRangedAttackEventArgs>? OnPlayerShoot;

	private void InitEquipment<T>(string? name, Func<string, T> creator, Action<T> equip)
	{
		if (name != null)
		{
			var equipment = creator.Invoke(name);
			equip.Invoke(equipment);
		}
	}
	
	public void Init(IPlayerState state, Direction direction,  Vector2I coordinate, IMap map, PlayerInfo info)
	{
		base.Init(info.Id, direction, coordinate, map, info.Name);
		IsMale = info.Male;
		_state = state;
		var equipmentFactory = EquipmentFactory.Instance;
		InitEquipment(info.WeaponName, n => equipmentFactory.CreatePlayerWeapon(n, IsMale), ChangeWeapon);
		InitEquipment(info.ChestName, n => equipmentFactory.CreatePlayerChest(n, IsMale), ChangeChest);
		InitEquipment(info.HairName, n => equipmentFactory.CreatePlayerHair(n, IsMale), ChangeHair);
		InitEquipment(info.HatName, n => equipmentFactory.CreatePlayerHat(n, IsMale), ChangeHat);
		InitEquipment(info.WristName, n => equipmentFactory.CreateWrist(n, IsMale, false), ChangeWrist);
		InitEquipment(info.BootName, n => equipmentFactory.CreateBoot(n, IsMale), ChangeBoot);
	}
	
	
	public PlayerWeapon? Weapon { get; private set; }
	
	public PlayerChest? Chest { get; private set; }
	
	public PlayerHair? Hair { get; private set; }
	
	public PlayerHat? Hat { get; private set; }
	
	public Wrist? Wrist { get; private set; }
	
	public Boot? Boot { get; private set; }
	
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

	public void ChangeHat(PlayerHat hat)
	{
		_hatAnimation = PlayerArmorAnimation.Create(hat);
		Hat = hat;
	}

	public void ChangeWrist(Wrist wrist)
	{
		_wristAnimation = PlayerArmorAnimation.Create(wrist);
		Wrist = wrist;
	}
	
	public void ChangeHair(PlayerHair hair)
	{
		_hairAnimation = PlayerArmorAnimation.Create(hair);
		Hair = hair;
	}
	
	public void ChangeBoot(Boot boot)
	{
		_bootAnimation = PlayerArmorAnimation.Create(boot);
		Boot = boot;
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
	
	public override OffsetTexture BodyOffsetTexture => _state.BodyOffsetTexture(this);
	public OffsetTexture? HandTexture => _handAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? ChestTexture => _chestAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? HairTexture => _hairAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? HatTexture => _hatAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? WristTexture => _wristAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? BootTexture => _bootAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);

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
		player.Init(state,
			interpolation.Direction, interpolation.Coordinate, map, playerInterpolation.PlayerInfo);
		if (state is PlayerMoveState moveState)
		{
			moveState.Init(player);
		}
		return player;
	}
}
