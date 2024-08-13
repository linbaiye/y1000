using System;
using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Character.Event;
using y1000.Source.Character.State;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Creature;
using y1000.Source.Event;
using y1000.Source.Input;
using y1000.Source.Item;
using y1000.Source.KungFu;
using y1000.Source.KungFu.Attack;
using y1000.Source.KungFu.Foot;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Player;
using y1000.Source.Util;
using ICharacterState = y1000.Source.Character.State.ICharacterState;

namespace y1000.Source.Character
{
	public partial class CharacterImpl : Node2D, IPlayer, IServerMessageVisitor, ICharacterMessageVisitor
	{
		private ICharacterState _state = EmptyState.Instance;

		private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

		public IFootKungFu? FootMagic { get; private set; }

		public IAttackKungFu AttackKungFu { get; private set; } = IAttackKungFu.Empty;
		
		public event EventHandler<EventArgs>? WhenCharacterUpdated;

		private EventMediator? EventMediator { get; set; }
		
		public ValueBar HealthBar { get; private set; } = ValueBar.Default;
		
		public ValueBar PowerBar { get; private set; } = ValueBar.Default;
		
		public ValueBar InnerPowerBar { get; private set; } = ValueBar.Default;
		
		public ValueBar OuterPowerBar { get; private set; } = ValueBar.Default;
		
		public ValueBar EnergyBar { get; private set; } = ValueBar.Default;

		public int HeadPercent { get; private set; } = 100;
		public int ArmPercent { get; private set; } = 100;
		public int LegPercent { get; private set; } = 100;

		public void ChangeState(ICharacterState state)
		{
			WrappedPlayer().ChangeState(state.WrappedState);
			_state = state;
		}

		public bool IsMale => WrappedPlayer().IsMale;
		public string EntityName => WrappedPlayer().EntityName;
		public PlayerWeapon ? Weapon => WrappedPlayer().Weapon;
		public PlayerChest? Chest =>  WrappedPlayer().Chest;
		public PlayerHair? Hair =>  WrappedPlayer().Hair;
		public PlayerHat? Hat =>  WrappedPlayer().Hat;
		public Wrist? Wrist =>  WrappedPlayer().Wrist;
		public Boot? Boot =>  WrappedPlayer().Boot;
		public Clothing? Clothing =>  WrappedPlayer().Clothing;
		public Trouser ? Trouser =>  WrappedPlayer().Trouser;
		public KungFuBook KungFuBook { get; private set; } = KungFuBook.Empty;
		
		public long Id => WrappedPlayer().Id;

		public Direction Direction
		{
			get => WrappedPlayer().Direction;
			
			set
			{
				{
					WrappedPlayer().Direction = value;
				}
			}
		}
		
		public string? ProtectionKungFu { get; set; }
		
		public string? AssistantKungFu { get; set; }
		
		public BreathKungFu? BreathKungFu { get; set; }

		public PlayerImpl WrappedPlayer()
		{
			foreach (var child in GetChildren())
			{
				if (child is PlayerImpl player)
				{
					return player;
				}
			}
			throw new NotImplementedException();
		}

		public Vector2I Coordinate => WrappedPlayer().Coordinate;

		public void Rewind(IPredictableResponse response)
		{
			//LOGGER.Debug("Need to rewind, message {0}.", response);
			if (response is MoveEventResponse { PositionMessage: RewindMessage rewind})
			{
				Visit(rewind);
			}
		}

		public void EmitPredictionEvent(IPrediction prediction, IClientEvent movementEvent)
		{
			WhenCharacterUpdated?.Invoke(this, new CharacterPredictionEventArgs(prediction, movementEvent));
		}

		private void NotifyServer(IClientEvent clientEvent)
		{
			EventMediator?.NotifyServer(clientEvent);
		}

		public void EmitMovedEvent()
		{
			WhenCharacterUpdated?.Invoke(this, new CharacterMoveEventArgs(Coordinate));
		}

		public bool CanMoveOneUnit(Direction direction)
		{
			var nextCoordinate = Coordinate.Move(direction);
			var ret = WrappedPlayer().Map.Movable(nextCoordinate);
			// LOGGER.Debug("Coordinate {0} {1} movable, current {2}", nextCoordinate, ret ? "is" : "is not", Coordinate);
			return ret;
		}
		
        private ICharacterState Rewind(CreatureState state)
        {
            if (state == CreatureState.COOLDOWN)
            {
	            return CharacterCooldownState.Cooldown();
            }
            if (state != CreatureState.IDLE)
            {
				LOGGER.Error("Not able to rewind to state {0}.", state);
            }
            return CharacterIdleState.Idle();
        }


        private void OnPlayerAnimationFinished(object? sender, CreatureAnimationDoneEventArgs args)
        {
	        _state.OnWrappedPlayerAnimationFinished(this);
        }

        private void SetPositionAndState(Vector2I coordinate, Direction direction, ICharacterState state)
        {
	        ChangeState(state);
	        WrappedPlayer().SetPosition(coordinate, direction);
	        EmitMovedEvent();
        }

        public void Visit(RewindMessage rewindMessage)
        {
	        SetPositionAndState(rewindMessage.Coordinate, rewindMessage.Direction, Rewind(rewindMessage.State));
        }

        public void Visit(SetPositionMessage message)
        {
	        if (message.State == CreatureState.DIE)
	        {
		        SetPositionAndState(message.Coordinate, message.Direction, CharacterDraggedState.Towards(message.Direction));
	        }
	        else
	        {
		        SetPositionAndState(message.Coordinate, message.Direction, ICharacterState.Create(message.State));
	        }
        }

        public void Handle(IEntityMessage message)
        {
	        //LOGGER.Debug("Received message {0}.", message);
	        message.Accept(this);
        }

        public void Visit(PlayerCooldownMessage message)
        {
	        ChangeState(CharacterCooldownState.Cooldown());
        }

        public void Visit(PlayerAttackMessage message)
        {
	        WrappedPlayer().UpdateAttackEffect(message);
	        SetPositionAndState(message.Coordinate, message.Direction, CharacterAttackState.FromMessage(message));
        }

        public void Visit(HurtMessage message)
        {
	        SetPositionAndState(message.Coordinate, message.Direction, CharacterHurtState.Hurt(message.AfterHurtState));
	        WrappedPlayer().ShowLifePercent(message.LifePercent);
	        WrappedPlayer().PlaySound(message.Sound);
	        HealthBar = new ValueBar(message.CurrentLife, message.MaxLife);
	        WhenCharacterUpdated?.Invoke(this, PlayerAttributeEvent.Instance);
        }
        public void Visit(EntitySoundMessage message)
        {
	        WrappedPlayer().Visit(message);
        }


        public void Visit(CreatureDieMessage message)
        {
	        ChangeState(ICharacterState.Create(CreatureState.DIE));
	        HealthBar = new ValueBar(0, HealthBar.Max);
	        WrappedPlayer().PlaySound(message.Sound);
	        WrappedPlayer().ShowLifePercent(HealthBar.Percent);
	        WhenCharacterUpdated?.Invoke(this, PlayerAttributeEvent.Instance);
	        EventMediator?.NotifyTextArea("请稍后");
        }
        
        public void Visit(PlayerUnequipMessage message)
        {
	        WrappedPlayer().Unequip(message);
	        if (!WrappedPlayer().IsHandAnimationCompatible)
	        {
		        ChangeState(CharacterCooldownState.Cooldown());
	        }
	        WhenCharacterUpdated?.Invoke(this, new EquipmentChangedEvent(message.Unequipped));
        }


        public void Visit(PlayerReviveMessage message)
        {
	        ChangeState(ICharacterState.Create(CreatureState.IDLE));
        }

        public void Visit(TeleportMessage message)
        {
	        WhenCharacterUpdated?.Invoke(this, new CharacterTeleportedArgs(message));
	        WrappedPlayer().SetPosition(message.Coordinate, WrappedPlayer().Direction);
        }

        public void Visit(UpdateKungFuSlotMessage message)
        {
	        KungFuBook.UpdateSlot(message.Slot, message.KungFu);
        }

        public void Handle(ICharacterMessage message)
        {
	        // LOGGER.Debug("Received message {0}.", message);
	        message.Accept(this);
        }


        private void Attack(AttackInput input)
        {
	        if (input.Entity.Id.Equals(Id) || !_state.CanAttack())
	        {
		        return;
	        }
	        var state = AttackKungFu.RandomAttackState();
	        Direction = Coordinate.GetDirection(input.Entity.Coordinate);
	        EmitPredictionEvent(new AttackPrediction(input, this), new CharacterAttackEvent(input, state, Direction));
	        var characterAttackState = CharacterAttackState.Attack(state);
	        ChangeState(characterAttackState);
        }

        public void HandleAttackResponse(CreatureState? backToState)
        {
	        if (backToState == null)
	        {
		        FootMagic = null;
		        WhenCharacterUpdated?.Invoke(this, KungFuChangedEvent.Instance);
	        }
	        else
	        {
		        ChangeState(ICharacterState.Create(backToState.Value));
	        }
        }

        private void HandleKeyInput(Key key)
        {
	        if (key == Key.F3 && _state.CanSitDown())
	        {
		        FootMagic = null;
		        ChangeState(CharacterSitDownState.SitDown());
		        NotifyServer(new ClientSitDownEvent(Coordinate));
		        WhenCharacterUpdated?.Invoke(this, KungFuChangedEvent.Instance);
	        }
	        else if (key == Key.F2 && _state.CanStandUp())
	        {
		        ChangeState(CharacterStandUpState.StandUp());
		        NotifyServer(new ClientStandEvent());
	        } else if (key == Key.F11)
	        {
		        NotifyServer(new ToggleKungFuEvent(1, 8));
	        }
        }

        public void Visit(PlayerSitDownMessage message)
        {
	        ChangeState(CharacterSitDownState.SitDown());
        }
        
        private void DispatchInput(IPredictableInput input)
        {
	        switch (input.Type)
	        {
		        case InputType.MOUSE_RIGHT_CLICK:
			        _state.OnMouseRightClicked(this, (MouseRightClick)input);
			        break;
		        case InputType.MOUSE_RIGHT_RELEASE:
			        _state.OnMouseRightReleased(this, (MouseRightRelease)input);
			        break;
		        case InputType.MOUSE_RIGHT_MOTION:
			        _state.OnMousePressedMotion(this, (RightMousePressedMotion)input);
			        break;
		        case InputType.ATTACK:
			        Attack((AttackInput)input);
			        break;
	        }
        }

        public void HandleInput(IPredictableInput input)
        {
	        if (input is KeyboardPredictableInput keyboardInput)
	        {
		        HandleKeyInput(keyboardInput.Key);
	        }
	        else if (_state.CanHandle(input))
	        {
		        DispatchInput(input);
	        }
        }


        private bool IsEquipped(EquipmentType type)
        {
	        return type switch
	        {
		        EquipmentType.CHEST => Chest != null,
		        EquipmentType.HAT => Hat != null,
		        EquipmentType.WEAPON => Weapon != null,
		        EquipmentType.CLOTHING => Clothing != null,
		        EquipmentType.BOOT => Boot != null,
		        EquipmentType.TROUSER => Trouser != null,
		        EquipmentType.WRIST => Wrist != null,
		        EquipmentType.WRIST_CHESTED => Wrist != null,
		        EquipmentType.HAIR => Hair != null,
		        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
	        };
        }

        public void OnAvatarDoubleClick(EquipmentType type)
        {
	        if (!IsEquipped(type))
	        {
		        return;
	        }
	        if (Inventory.IsFull)
	        {
		        EventMediator?.NotifyTextArea("物品栏已满");
		        return;
	        }
	        EventMediator?.NotifyServer(new ClientUnequipEvent(type));
        }

        public void OnAvatarRightClick()
        {
	        EventMediator?.NotifyServer(new ClientRightClickEvent(RightClickType.CHARACTER));
        }
        

        public CharacterInventory Inventory { get; } = new();

        private static void AddItems(CharacterImpl characterImpl, JoinedRealmMessage message, ItemFactory itemFactory)
        {
	        foreach (var inventoryItemMessage in message.Items)
	        {
		        var characterItem = itemFactory.CreateCharacterItem(inventoryItemMessage);
		        characterImpl.Inventory.PutItem(inventoryItemMessage.SlotId, characterItem);
	        }
        }

  

        public OffsetTexture BodyOffsetTexture => WrappedPlayer().BodyOffsetTexture;
        
        public Vector2 OffsetBodyPosition => WrappedPlayer().OffsetBodyPosition;

        public void Visit(SwapInventorySlotMessage message)
        {
	        Inventory.Swap(message.Slot1, message.Slot2);
        }

        public void Visit(GainExpMessage message)
        {
	        if (message.IsKungFu)
		        KungFuBook.GainExp(message.Name, message.Level);
	        WhenCharacterUpdated?.Invoke(this, new GainExpEventArgs(message.Name, message.IsKungFu));
        }

        public void Visit(PlayerLearnKungFuMessage message)
        {
	        KungFuBook.Add(message.SlotId, message.KungFu);
	        LOGGER.Debug("Learned kungfu {}.", message.KungFu);
	        WhenCharacterUpdated?.Invoke(this, LearnKungFuEventArgs.Instance);
        }

        public void Visit(KungFuOrItemAttributeMessage message)
        {
	        if (message.Type == RightClickType.INVENTORY)
	        {
		        var characterItem = Inventory.Find(message.SlotId);
		        if (characterItem != null)
		        {
			        EventMediator?.NotifyUiEvent(new ItemAttributeEvent(characterItem, message.Description));
		        }
	        } else if (message.Type == RightClickType.KUNGFU)
	        {
		        var kungFu = KungFuBook.Get(message.Page, message.SlotId);
		        if (kungFu != null)
		        {
			        EventMediator?.NotifyUiEvent(new KungFuAttributeEvent(kungFu, message.Description));
		        }
	        }
        }

        public void Visit(PlayerAttributeMessage message)
        {
	        EventMediator?.NotifyUiEvent(message);
        }

        public void Visit(DropItemMessage message)
        {
	        Inventory.DropItem(message.Slot, message.NumberLeft);
        }

        public void Visit(UpdateInventorySlotMessage message)
        {
	        Inventory.Update(message.SlotId, message.Item);
        }

        public void Visit(CharacterAttributeMessage message)
        {
	        HealthBar = message.Health;
	        PowerBar = message.Power;
	        InnerPowerBar = message.InnerPower;
	        OuterPowerBar = message.OuterPower;
	        HeadPercent = message.HeadPercent;
	        ArmPercent = message.ArmPercent;
	        LegPercent = message.LegPercent;
	        WhenCharacterUpdated?.Invoke(this, PlayerAttributeEvent.Instance);
        }

        public void Visit(PlayerEquipMessage message)
        {
	        var equipment = WrappedPlayer().Equip(message);
	        if (!WrappedPlayer().IsHandAnimationCompatible)
	        {
		        ChangeState(CharacterCooldownState.Cooldown());
	        }
	        WhenCharacterUpdated?.Invoke(this ,new EquipmentChangedEvent(equipment.EquipmentType));
        }

        private void ToggleProtectionKungFu(PlayerToggleKungFuMessage message)
        {
	        ProtectionKungFu = message.Level > 0 ? message.Name : null;
        }

        public bool Dead => _state.WrappedState.State == CreatureState.DIE;

        private void ToggleBreathKungFu(PlayerToggleKungFuMessage message)
        {
	        BreathKungFu = message.Level > 0 ? KungFuBook.FindKungFu<BreathKungFu>(message.Name) : null;
        }

        private void ToggleFootKungFu(PlayerToggleKungFuMessage message)
        {
	        FootMagic = message.Level > 0? KungFuBook.FindKungFu<IFootKungFu>(message.Name) : null;
        }
        
        public void Visit(PlayerToggleKungFuMessage message)
        {
	        WrappedPlayer().Visit(message);
	        switch (message.Type)
	        {
		        case KungFuType.FOOT:
			        ToggleFootKungFu(message);
			        break;
		        case KungFuType.PROTECTION:
			        ToggleProtectionKungFu(message);
			        break;
		        case KungFuType.ASSISTANT:
			        AssistantKungFu = message.Level > 0 ? message.Name : null;
			        break;
		        case KungFuType.BREATHING:
			        ToggleBreathKungFu(message);
			        break;
		        case KungFuType.AXE:
			        AttackKungFu = KungFuBook.FindKungFu<AxeKungFu>(message.Name);
			        break;
		        case KungFuType.BLADE:
			        AttackKungFu = KungFuBook.FindKungFu<BladeKungFu>(message.Name);
			        break;
		        case KungFuType.SWORD:
			        AttackKungFu = KungFuBook.FindKungFu<SwordKungFu>(message.Name);
			        break;
		        case KungFuType.SPEAR:
			        AttackKungFu = KungFuBook.FindKungFu<SpearKungFu>(message.Name);
			        break;
		        case KungFuType.BOW:
			        AttackKungFu = KungFuBook.FindKungFu<BowKungFu>(message.Name);
			        break;
		        case KungFuType.QUANFA:
			        AttackKungFu = KungFuBook.FindKungFu<QuanFa>(message.Name);
			        break;
		        case KungFuType.THROW:
			        AttackKungFu = KungFuBook.FindKungFu<ThrowKungFu>(message.Name);
			        break;
		        default:
			        throw new ArgumentOutOfRangeException();
	        }
	        WhenCharacterUpdated?.Invoke(this ,KungFuChangedEvent.Instance);
        }

        public void Visit(PlayerStandUpMessage message)
        {
	        ChangeState(CharacterStandUpState.StandUp());
        }

        public void DropItemOnPlayer(MessageDrivenPlayer player, int inventorySlot)
        {
	        if (Dead)
	        {
		        return;
	        }
	        if (player.Dead)
	        {
		        if (Inventory.HasItem(inventorySlot) && Inventory.GetOrThrow(inventorySlot).ItemName.Equals("追魂索"))
		        {
			        EventMediator?.NotifyServer(new ClientDragPlayerEvent(player.Id, inventorySlot));
		        }
		        return;
	        }
	        if (player.CanBeTraded(this) && Inventory.HasItem(inventorySlot))
	        {
		        EventMediator?.NotifyServer(new ClientTradePlayerEvent(player.Id, inventorySlot));
	        }
	        else
	        {
		        EventMediator?.NotifyTextArea("距离过远。");
	        }
        }


        public static CharacterImpl LoggedIn(JoinedRealmMessage message,
	        IMap map,  ItemFactory itemFactory, EventMediator eventMediator)
        {
	        PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/character.tscn");
	        var character = scene.Instantiate<CharacterImpl>();
	        var state = IPlayerState.Idle();
	        var player = character.WrappedPlayer();
	        player.Init(state, Direction.DOWN, message.Coordinate, map, message.MyInfo);
	        player.StateAnimationEventHandler += character.OnPlayerAnimationFinished;
	        character.FootMagic = message.FootKungFu;
	        character.AttackKungFu = message.AttackKungFu;
	        character.Inventory.SetEventMediator(eventMediator);
	        character.EventMediator = eventMediator;
	        character.ChangeState(CharacterIdleState.Wrap(state));
	        character.KungFuBook = message.KungFuBook;
	        character.KungFuBook.EventMediator = eventMediator;
	        character.ProtectionKungFu = message.ProtectionKungFu;
	        character.AssistantKungFu = message.AssistantKungFu;
	        character.HealthBar = message.HealthBar;
	        character.PowerBar = message.PowerBar;
	        character.InnerPowerBar = message.InnerPowerBar;
	        character.OuterPowerBar = message.OuterPowerBar;
	        character.EnergyBar = message.EnergyBar;
	        character.HeadPercent = message.HeadPercent;
	        character.ArmPercent = message.ArmPercent;
	        character.LegPercent = message.LegPercent;
	        AddItems(character, message, itemFactory);
	        return character;
        }

        public Rect2 BodyRectangle => WrappedPlayer().BodyRectangle;
	}
}
