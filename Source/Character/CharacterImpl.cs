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

		public KungFuBook KungFuBook { get; set; } = KungFuBook.Empty;
		
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
			LOGGER.Debug("Need to rewind, message {0}.", response);
			if (response is MoveEventResponse { PositionMessage: RewindMessage rewind})
			{
				Visit(rewind);
			}
		}

		public void EmitPredictionEvent(IPrediction prediction, IClientEvent movementEvent)
		{
			WhenCharacterUpdated?.Invoke(this, new CharacterPredictionEventArgs(prediction, movementEvent));
		}

		public void EmitMoveEvent()
		{
			WhenCharacterUpdated?.Invoke(this, new CharacterMoveEventArgs(Coordinate));
		}

		public bool CanMoveOneUnit(Direction direction)
		{
			return WrappedPlayer().Map.Movable(Coordinate.Move(direction));
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
	        WrappedPlayer().SetPosition(coordinate, direction);
	        ChangeState(state);
	        EmitMoveEvent();
        }

        public void Visit(RewindMessage rewindMessage)
        {
	        SetPositionAndState(rewindMessage.Coordinate, rewindMessage.Direction, Rewind(rewindMessage.State));
        }

        public void Visit(SetPositionMessage setPositionMessage)
        {
	        SetPositionAndState(setPositionMessage.Coordinate, setPositionMessage.Direction, Rewind(setPositionMessage.State));
        }

        public void Handle(IEntityMessage message)
        {
	        LOGGER.Debug("Received message {0}.", message);
	        message.Accept(this);
        }

        public void Visit(PlayerAttackMessage message)
        {
	        SetPositionAndState(message.Coordinate, message.Direction, CharacterAttackState.FromMessage(message));
        }

        public void Visit(HurtMessage message)
        {
	        SetPositionAndState(message.Coordinate, message.Direction, CharacterHurtState.Hurt(message.AfterHurtState));
        }

        public void Visit(PlayerUnequipMessage message)
        {
	        WrappedPlayer().Visit(message);
	        if (message.NewState != _state.WrappedState.State && message.NewState != null)
	        {
		        LOGGER.Debug("Changed state to cooldown.");
		        ChangeState(CharacterCooldownState.Cooldown());
		        AttackKungFu = new QuanFa(message.QuanfaLevel, "无名拳法");
	        }
	        WhenCharacterUpdated?.Invoke(this, new EquipmentChangedEvent(message.Unequipped));
        }

        public void Handle(ICharacterMessage message)
        {
	        message.Accept(this);
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
			        _state.Attack(this, (AttackInput)input);
			        break;
		        default:
			        throw new ArgumentOutOfRangeException();
	        }
        }

        public void HandleInput(IPredictableInput input)
        {
	        if (_state.CanHandle(input))
	        {
		        DispatchInput(input);
	        }
        }


        private bool IsEquiped(EquipmentType type)
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
	        if (!IsEquiped(type))
	        {
		        return;
	        }
	        if (Inventory.IsFull)
	        {
		        EventMediator?.NotifyMessage("物品栏已满");
		        return;
	        }
	        EventMediator?.NotifyServer(new ClientUnequipEvent(type));
        }
        

        public CharacterInventory Inventory { get; } = new();

        private static void AddItems(CharacterImpl characterImpl, JoinedRealmMessage message, ItemFactory itemFactory)
        {
	        foreach (var inventoryItemMessage in message.Items)
	        {
		        var characterItem = itemFactory.CreateCharacterItem(inventoryItemMessage);
		        characterImpl.Inventory.AddItem(inventoryItemMessage.SlotId, characterItem);
	        }
        }

        public OffsetTexture BodyOffsetTexture => WrappedPlayer().BodyOffsetTexture;
        
        public Vector2 OffsetBodyPosition => WrappedPlayer().OffsetBodyPosition;

        public void Visit(SwapInventorySlotMessage message)
        {
	        Inventory.Swap(message.Slot1, message.Slot2);
        }
        
        public void Visit(CharacterChangeWeaponMessage message)
        {
	        var weapon = EquipmentFactory.Instance.CreatePlayerWeapon(message.WeaponName, IsMale);
	        WrappedPlayer().ChangeWeapon(weapon);
	        if (message.State != _state.WrappedState.State)
	        {
		        ChangeState(CharacterCooldownState.Cooldown());
	        }
	        if (message.AttackKungFu != null)
		        AttackKungFu = message.AttackKungFu;
	        Inventory.PutOrRemove(message.AffectedSlotId, message.NewItem);
			WhenCharacterUpdated?.Invoke(this, new WeaponChangedEvent(weapon));
        }

        public void Visit(DropItemMessage message)
        {
	        Inventory.DropItem(message.Slot, message.NumberLeft);
        }

        public void Visit(UpdateInventorySlotMessage message)
        {
	        Inventory.Update(message.SlotId, message.Item);
        }

        public void Visit(PlayerEquipMessage message)
        {
	        var equipment = WrappedPlayer().Equip(message);
	        WhenCharacterUpdated?.Invoke(this ,new EquipmentChangedEvent(equipment.EquipmentType));
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
	        AddItems(character, message, itemFactory);
	        return character;
        }
	}
}
