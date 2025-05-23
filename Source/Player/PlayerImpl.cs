using System;
using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Control;
using y1000.Source.Creature;
using y1000.Source.Item;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Util;

namespace y1000.Source.Player;

public partial class PlayerImpl: AbstractCreature, IPlayer, IServerMessageVisitor, IPlayerAnimation
{
	private IPlayerState _state = IPlayerState.Empty;

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	private PlayerWeaponAnimation? _handAnimation;

	private PlayerArmorAnimation? _chestAnimation;
	
	private PlayerArmorAnimation? _hairAnimation;
	
	private PlayerArmorAnimation? _hatAnimation;
	
	private PlayerArmorAnimation? _leftWristAnimation;
	
	private PlayerArmorAnimation? _rightWristAnimation;
	
	private PlayerArmorAnimation? _bootAnimation;
	
	private PlayerArmorAnimation? _trouserAnimation;
	
	private PlayerArmorAnimation? _clothingAnimation;
	
	private WeaponEffectAnimation? _effectAnimation;


	private void InitEquipment<T>(string? name, Func<string, T> creator, Action<T> equip)
	{
		if (name != null)
		{
			var equipment = creator.Invoke(name);
			equip.Invoke(equipment);
		}
	}
	
	private void InitEquipment<T>(string? name, int c, Action<T> equip) where T : IEquipment
	{
		var equipmentFactory = EquipmentFactory.Instance;
		if (name != null)
		{
			var equipment = equipmentFactory.Create<T>(name, IsMale, c);
			equip.Invoke(equipment);
		}
	}

	private void InitWrist(string? name, int color)
	{
		if (name != null)
		{
			var wrist = EquipmentFactory.Instance.CreateWrist(name, IsMale, color);
			ChangeWrist(wrist);
		}
	}
	
	public void Init(IPlayerState state, Direction direction,  Vector2I coordinate, IMap map, PlayerInfo info)
	{
		base.Init(info.Id, direction, coordinate, map, info.Name);
		IsMale = info.Male;
		_state = state;
		var equipmentFactory = EquipmentFactory.Instance;
		InitEquipment(info.WeaponName, n => equipmentFactory.CreatePlayerWeapon(n, IsMale), ChangeWeapon);
		InitEquipment<PlayerChest>(info.ChestName, info.ChestColor, ChangeChest);
		InitEquipment<PlayerHair>(info.HairName,  info.HairColor, ChangeHair);
		InitEquipment<PlayerHat>(info.HatName, info.HatColor, ChangeHat);
		InitWrist(info.WristName, info.WristColor);
		InitEquipment<Boot>(info.BootName, info.BootColor, ChangeBoot);
		InitEquipment<Trouser>(info.TrouserName, info.TrouserColor, ChangeTrouser);
		InitEquipment<Clothing>(info.ClothingName, info.ClothingColor, ChangeClothing);
		SetNameColor(info.NameColor);
		SetGuildName(info.GuildName);
	}

	public PlayerWeapon? Weapon { get; private set; }
	
	public PlayerChest? Chest { get; private set; }
	
	public PlayerHair? Hair { get; private set; }
	
	public PlayerHat? Hat { get; private set; }
	
	public Wrist? Wrist { get; private set; }
	
	public Boot? Boot { get; private set; }
	
	public Trouser? Trouser { get; private set; }
	
	public Clothing? Clothing { get; private set; }

	private void ChangeWeapon(PlayerWeapon? weapon)
	{
		_handAnimation = weapon != null ? PlayerWeaponAnimation.LoadFor(weapon) : null;
		Weapon = weapon;
		GetNode<Sprite2D>("Hand").Visible = weapon != null;
	}

	private void ChangeChest(PlayerChest? chest)
	{
		_chestAnimation = chest != null ? PlayerArmorAnimation.Create(chest) : null;
		Chest = chest;
		GetNode<ChestSprite>("Chest").OnEquipmentChanged(chest);
	}

	private void ChangeHat(PlayerHat? hat)
	{
		_hatAnimation = hat != null ? PlayerArmorAnimation.Create(hat) : null;
		Hat = hat;
		GetNode<HatSprite>("Hat").OnEquipmentChanged(hat);
	}

	private void ChangeWrist(Wrist? wrist)
	{
		Wrist = wrist;
		_leftWristAnimation = wrist != null ? PlayerArmorAnimation.CreateLeftWrist(wrist) : null;
		_rightWristAnimation = wrist != null ? PlayerArmorAnimation.CreateRightWrist(wrist) : null;
		GetNode<LeftWristSprite>("LeftWrist").OnEquipmentChanged(wrist);
		GetNode<RightWristSprite>("RightWrist").OnEquipmentChanged(wrist);
	}

	private void ChangeHair(PlayerHair? hair)
	{
		_hairAnimation = hair != null ? PlayerArmorAnimation.Create(hair) : null;
		Hair = hair;
		GetNode<HairSprite>("Hair").OnEquipmentChanged(hair);
	}

	private void ChangeBoot(Boot? boot)
	{
		_bootAnimation = boot != null ? PlayerArmorAnimation.Create(boot) : null;
		Boot = boot;
		GetNode<BootSprite>("Boot").OnEquipmentChanged(boot);
	}

	private void ChangeClothing(Clothing? clothing)
	{
		_clothingAnimation = clothing != null ? PlayerArmorAnimation.Create(clothing) : null;
		Clothing = clothing;
		GetNode<ClothingSprite>("Clothing").OnEquipmentChanged(clothing);
	}

	private void ChangeTrouser(Trouser? trouser)
	{
		_trouserAnimation = trouser != null ? PlayerArmorAnimation.Create(trouser) : null;
		Trouser = trouser;
		GetNode<TrouserSprite>("Trouser").OnEquipmentChanged(trouser);
	}

	public bool IsHandAnimationCompatible => _handAnimation == null || _handAnimation.Compatible(_state.State);


	public void Unequip(PlayerUnequipMessage message)
	{
		switch (message.Unequipped)
		{
			case EquipmentType.TROUSER: ChangeTrouser(null); break;
			case EquipmentType.HAT: ChangeHat(null); break;
			case EquipmentType.CLOTHING: ChangeClothing(null); break;
			case EquipmentType.HAIR: ChangeHair(null); break;
			case EquipmentType.BOOT: ChangeBoot(null); break;
			case EquipmentType.CHEST : ChangeChest(null); break;
			case EquipmentType.WRIST: 
			case EquipmentType.WRIST_CHESTED: ChangeWrist(null); break;
			case EquipmentType.WEAPON: ChangeWeapon(null);
				break;
		}
	}

	public void Visit(PlayerUnequipMessage message)
	{
		Unequip(message);
		if (!IsHandAnimationCompatible)
		{
			ChangeState(IPlayerState.Cooldown());
		}
	}

	public void Visit(PlayerChangeNameColorMessage message)
	{
		SetNameColor(message.Color);
	}

	private void SetNameColor(int color) {
		var code = color <= 0 ? "ffffff" : color.ToString("X6");
		NameColor = code;
		BodySprite?.UpdateColor();
	}
	
	private void SetGuildName(string? guildName)
	{
		GuildName = guildName;
		BodySprite?.UpdateGuild();
	}

	public string? GuildName { get; private set; }
	public string NameColor { get; private set; } = "ffffff";

	public IEquipment Equip(PlayerEquipMessage message)
	{
		var equipment = EquipmentFactory.Instance.Create(message.EquipmentName, IsMale, message.Color);
		switch (equipment)
		{
			case Trouser trouser: ChangeTrouser(trouser);
				break;
			case PlayerHat hat: ChangeHat(hat);
				break;
			case Clothing clothing: ChangeClothing(clothing);
				break;
			case PlayerHair hair: ChangeHair(hair);
				break;
			case Boot boot: ChangeBoot(boot);
				break;
			case PlayerChest chest: ChangeChest(chest);
				break;
			case Wrist wrist: ChangeWrist(wrist);
				break;
			case PlayerWeapon weapon : ChangeWeapon(weapon);
				break;
		}

		return equipment;
	}
	
	public void Visit(PlayerEquipMessage message)
	{
		Equip(message);
		if (!IsHandAnimationCompatible)
		{
			ChangeState(IPlayerState.Cooldown());
		}
	}

	public void Visit(PlayerUpdateGuildMessage message)
	{
		SetGuildName(message.Name);
	}

	public void Visit(PlayerStandUpMessage message)
	{
		ChangeState(IPlayerState.NonHurtState(CreatureState.STANDUP));
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
		if (_state is PlayerMoveState moveState && moveState.ToCoordinate.HasValue)
		{
			if (!Position.ToCoordinate().Equals(moveState.ToCoordinate.Value))
			{
				Position = moveState.ToCoordinate.Value.ToPosition();
			}
			Map.Occupy(this);
		}
		_state = newState;
	}

	public bool Dead => _state.State == CreatureState.DIE;

	public void Visit(PlayerToggleKungFuMessage message)
	{
		if (!message.Quietly)
			DisplayDialog(message.Name);
	}

	public void Visit(PlayerMoveMessage message)
	{
		SetPosition(message.Coordinate, message.Direction);
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

	public void SlowCamera()
	{
		GetNode<Camera2D>("MainCamera").Enabled = false;
		GetNode<Camera2D>("DraggedCamera").Enabled = true;
	}

	private void LimitCamera(Camera2D camera, Vector2I leftTop, Vector2I bottomRight)
	{
		camera.LimitLeft = leftTop.X;
		camera.LimitTop = leftTop.Y;
		camera.LimitRight = bottomRight.X;
		camera.LimitBottom = bottomRight.Y;
	}

	public void LimitCamera(Vector2I leftTop, Vector2I bottomRight)
	{
		LimitCamera(GetNode<Camera2D>("MainCamera"), leftTop, bottomRight);
		LimitCamera(GetNode<Camera2D>("DraggedCamera"), leftTop, bottomRight);
	}

	public void RestoreCamera()
	{
		GetNode<Camera2D>("MainCamera").Enabled = true;
		GetNode<Camera2D>("DraggedCamera").Enabled = false;
	}

	public void Visit(DraggedMessage draggedMessage)
	{
		SetPosition(draggedMessage);
	}

	public void Handle(IEntityMessage message)
	{
		message.Accept(this);
	}

	public void Visit(RemoveEntityMessage message)
	{
		Delete();
	}

	public void Visit(PlayerSitDownMessage message)
	{
		ChangeState(IPlayerState.NonHurtState(CreatureState.SIT));
	}
	
	public void ResetState()
	{
		_state.Reset();
	}
	
	public void Visit(PlayerCooldownMessage message)
	{
		ChangeState(IPlayerState.NonHurtState(CreatureState.COOLDOWN));
	}

	public void Visit(PlayerReviveMessage message)
	{
		ChangeState(IPlayerState.Idle());
	}

	
	public override OffsetTexture BodyOffsetTexture => _state.BodyOffsetTexture(this);
	public OffsetTexture? HandTexture => _handAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? ChestTexture => _chestAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? HairTexture => _hairAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? HatTexture => _hatAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? LeftWristTexture => _leftWristAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? RightWristTexture => _rightWristAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? BootTexture => _bootAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? ClothingTexture => _clothingAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? TrouserTexture => _trouserAnimation?.OffsetTexture(_state.State, Direction, _state.ElapsedMillis);
	public OffsetTexture? AttackEffect => _effectAnimation?.GetIfAttacking(_state.State, Direction, _state.ElapsedMillis);


	public void UpdateAttackEffect(PlayerAttackMessage message)
	{
		if (message.EffectId != 0)
		{
			if (_effectAnimation == null || _effectAnimation.EffectId != message.EffectId)
				_effectAnimation = WeaponEffectAnimation.LoadFor(message.State, message.EffectId);
		}
		else
		{
			_effectAnimation = null;
		}
	}

	public void Visit(PlayerAttackMessage message)
	{
		SetPosition(message.Coordinate, message.Direction);
		UpdateAttackEffect(message);
		ChangeState(IPlayerState.Attack(message));
	}

	public void Visit(HurtMessage hurtMessage)
	{
		ChangeState(IPlayerState.Hurt(hurtMessage.AfterHurtState));
		HandleHurt(hurtMessage);
	}

	public void Visit(CreatureDieMessage message)
	{
		ChangeState(IPlayerState.NonHurtState(CreatureState.DIE));
		ShowLifePercent(0);
	}

	public void Visit(EntityChatMessage message)
	{
		DisplayDialog(message.Content);
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
			moveState.DriftPosition(player);
		}
		LOGGER.Debug("Created player {0} at {1}, visible {2}.", player.Id, player.Position, player.Visible);
		return player;
	}

	public Rect2 BodyRectangle
	{
		get
		{
			var bodySprite = GetNode<BodySprite>("Body");
			var position = Position;
			position += bodySprite.Offset;
			var size = bodySprite.Texture.GetSize();
			return new Rect2(position, size);
		}
	}
}
