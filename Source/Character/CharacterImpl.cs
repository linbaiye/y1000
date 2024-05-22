using System;
using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Character.Event;
using y1000.Source.Character.State;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Creature;
using y1000.Source.Input;
using y1000.Source.Item;
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
	public partial class CharacterImpl : Node2D, IPlayer, IServerMessageVisitor
	{
		private ICharacterState _state = EmptyState.Instance;

		private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

		public IFootKungFu? FootMagic { get; private set; }

		public IAttackKungFu AttackKungFu { get; private set; } = IAttackKungFu.Empty;

		public event EventHandler<EventArgs>? WhenCharacterUpdated;

		public void ChangeState(ICharacterState state)
		{
			WrappedPlayer().ChangeState(state.WrappedState);
			_state = state;
		}

		private void OnInventoryEvent(IClientEvent clientEvent)
		{
			
		}

		public bool IsMale => WrappedPlayer().IsMale;
		
		public OffsetTexture? HandTexture => WrappedPlayer().HandTexture;

		public string EntityName => WrappedPlayer().EntityName;
		
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

        public CharacterImpl()
        {
	        Inventory = new CharacterInventory(OnInventoryEvent);
        }

        public CharacterInventory Inventory { get; }

        private static void AddItems(CharacterImpl characterImpl, JoinedRealmMessage message, ItemFactory itemFactory)
        {
	        foreach (var inventoryItemMessage in message.Items)
	        {
		        var characterItem = itemFactory.CreateCharacterItem(inventoryItemMessage.Id, inventoryItemMessage.Name, inventoryItemMessage.Type);
		        characterImpl.Inventory.AddItem(inventoryItemMessage.SlotId, characterItem);
	        }
        }

        public static CharacterImpl LoggedIn(JoinedRealmMessage message,
	        IMap map,  ItemFactory itemFactory)
        {
	        PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/character.tscn");
	        var character = scene.Instantiate<CharacterImpl>();
	        var state = IPlayerState.Idle();
	        var player = character.WrappedPlayer();
	        player.Init(message.Male, state, Direction.DOWN, message.Coordinate, message.Id, map);
	        if (message.WeaponName != null)
	        {
		        var weapon = ItemFactory.Instance.CreatePlayerWeapon(message.WeaponName);
		        player.ChangeWeapon(weapon);
	        }
	        player.StateAnimationEventHandler += character.OnPlayerAnimationFinished;
	        character.FootMagic = message.FootKungFu;
	        character.AttackKungFu = message.AttackKungFu;
	        character.ChangeState(CharacterIdleState.Wrap(state));
	        AddItems(character, message, itemFactory);
	        return character;
        }

        public OffsetTexture BodyOffsetTexture => WrappedPlayer().BodyOffsetTexture;

        public Vector2 BodyPosition => WrappedPlayer().BodyPosition;
	}
}
