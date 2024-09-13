using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Godot;
using NLog;
using y1000.Source.Assistant;
using y1000.Source.Audio;
using y1000.Source.Character;
using y1000.Source.Character.Event;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Control;
using y1000.Source.Creature;
using y1000.Source.Creature.Monster;
using y1000.Source.Entity;
using y1000.Source.Event;
using y1000.Source.Input;
using y1000.Source.Item;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Connection;
using y1000.Source.Networking.Server;
using y1000.Source.Player;
using y1000.Source.Sprite;

namespace y1000.Source;

public partial class Game : Node2D, IConnectionEventListener, IServerMessageVisitor
{
	private readonly Bootstrap _bootstrap = new();

	private readonly ConcurrentQueue<object> _unprocessedMessages = new();

	private readonly EntityManager _entityManager;
	
	private readonly InputSampler _inputSampler = new();

	private readonly PredictionManager _predictionManager = new();

	private CharacterImpl? _character;

	private volatile IChannel? _channel;

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	private UIController? _uiController;

	private readonly ItemFactory _itemFactory;

	private readonly MessageFactory _messageFactory;

	private readonly EntityFactory _entityFactory;

	private readonly EventMediator _eventMediator;

	private readonly ISpriteRepository _spriteRepository;
	
	private AudioManager? _audioManager;

	private AutoFillAssistant? _autoFillAssistant;
	private AutoLootAssistant? _autoLootAssistant;

	private string _token = "";
	
	private string _charName = "";

	public Game()
	{
		// 1024, 768
		_eventMediator = InitializeEventMediator();
		_itemFactory = ItemFactory.Instance;
		_messageFactory = new MessageFactory(_itemFactory);
		_entityManager = EntityManager.Instance;
		_spriteRepository = ISpriteRepository.Instance;;
		_entityFactory = new EntityFactory(_eventMediator, _spriteRepository);
		_character = null;
	}

	private EventMediator InitializeEventMediator()
	{
		var eventMediator = new EventMediator();
		eventMediator.SetComponent(WriteMessage);
		eventMediator.SetComponent(OnDragItemEvent);
		return eventMediator;
	}

	public void SetToken(string t, string n)
	{
		_token = t;
		_charName = n;
	}

	public override void _Ready()
	{
		SetupNetwork();
		_uiController = GetNode<UIController>("UILayer");
		_uiController.Initialize(_eventMediator, _spriteRepository, _itemFactory);
		GetWindow().Size = new Vector2I(1024, 768);
		GetWindow().ContentScaleSize = new Vector2I(1024, 768);
		_audioManager = GetNode<AudioManager>("AudioManager");
	}

	private void OnCharacterEvent(object? sender, EventArgs eventArgs)
	{
		if (sender is not CharacterImpl || _character == null)
		{
			return;
		}

		if (eventArgs is CharacterPredictionEventArgs predictionEventArgs)
		{
			_predictionManager.Save(predictionEventArgs.Prediction);
			WriteMessage(predictionEventArgs.Event);
		}
		else if (eventArgs is CharacterTeleportedArgs teleportedArgs)
		{
			AddChild(_character);
			_entityManager.Add(_character);
			_audioManager?.LoadAndPlayBackgroundMusic(teleportedArgs.Bgm);
		}
	}

	private async void SetupNetwork()
	{
		_bootstrap.Group(new MultithreadEventLoopGroup()).Handler(
				new ActionChannelInitializer<ISocketChannel>(c => c.Pipeline.AddLast(
				new LengthFieldPrepender(4), 
				new MessageEncoder(),
				new LengthBasedPacketDecoder(_messageFactory),
				new MessageHandler(this))
				)).Channel<TcpSocketChannel>();
		_channel = await _bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));
		await _channel.WriteAndFlushAsync(new LoginEvent(_token, _charName));
	}

	private async void WriteMessage(IClientEvent message)
	{
		await Task.Run(() =>
		{
			Thread.Sleep(50);
		});
		_channel?.WriteAndFlushAsync(message);
	}


	private MapLayer MapLayer => GetNode<MapLayer>("MapLayer");

	private void LocalTest(InputEventKey eventKey)
	{
		if (_character == null || !eventKey.IsPressed())
		{
			return;
		}

		if (eventKey.Keycode == Key.M )
		{
			_uiController?.ToggleMap(MapLayer);
		}

		/*if (eventKey.Keycode >= Key.Key1 && eventKey.Keycode <= Key.Key9)
		{
			var index = (ColorType)(eventKey.Keycode - Key.Key1);
			for (int i = 0; i < _entitySoundPlayers.Length; i++)
			{
				if (!_entitySoundPlayers[i].Playing)
				{
					var name = (8950 + index).ToString();
					LOGGER.Debug("Play sound {} with player {}", name, i);
					_entitySoundPlayers[i].PlaySound(name);
					break;
				}
			}
		}*/
	}


	public void Visit(EntitySoundMessage message)
	{
		if (message.Id == _character.Id)
		{
			_audioManager?.PlayCharacterSound(message.Sound);
		}
		else
		{
			_audioManager?.PlaySound(message);
		}
	}
	

	
	private void HandleInput(InputEvent @event)
	{
		if (_character == null)
		{
			return;
		}

		if (@event is InputEventKey eventKey)
		{
			LocalTest(eventKey);
			//return;//
		}
		var mousePos = _character.WrappedPlayer().GetLocalMousePosition();
		var predictableInput = _inputSampler.SampleInput(@event, mousePos);
		if (predictableInput != null)
		{
			_character.HandleInput(predictableInput);
		}
	}
	

	public override void _UnhandledInput(InputEvent @event)
	{
		HandleInput(@event);
	}


	public override void _Process(double delta)
	{
		HandleMessages();
		_autoFillAssistant?.Process(delta);
		_autoLootAssistant?.Process(delta);
	}


	private void HandleMessages()
	{
		while (_unprocessedMessages.TryDequeue(out var message))
		{
			if (message is not IServerMessage serverMessage)
			{
				continue;
			}
			try
			{
				serverMessage.Accept(this);
			}
			catch (Exception e)
			{
				LOGGER.Error(e, "Caught exception." );
			}
		}
	}

	private void OnEntityClicked(object? sender, EntityMouseEventArgs args)
	{
		if (_character == null)
		{
			return;
		}
		var click = _inputSampler.SampleEntityClickInput(args.MouseEvent, args.Entity, 
			_character.WrappedPlayer().GetLocalMousePosition());
		if (click is AttackInput attack)
		{
			_character?.HandleInput(attack);
		}
        else if (args.Entity is Merchant merchant && click is CreatureLeftClick && !merchant.IsDead)
		{
			_uiController?.OnMerchantClicked(merchant);
		}
		else if (click is IPredictableInput predictableInput)
		{
			_character?.HandleInput(predictableInput);
		}
		else if (args.Entity is Monster monster && monster.Type == NpcType.BANKER &&
		         click is CreatureLeftClick)
		{
			_eventMediator.NotifyServer(ClientOperateBankEvent.Open(args.Entity.Id));
		}
	}


	public void OnMessageArrived(object message)
	{
		_unprocessedMessages.Enqueue(message);
	}

	private async void Reconnect()
	{
		await Task.Run(() =>
		{
			Task.Delay(2000);
		});
		try
		{
			_channel?.CloseAsync();
			_channel = await _bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));
		}
		catch (Exception)
		{
			Reconnect();
		}
	}

	public void OnConnectionClosed()
	{
		_channel = null;
		_unprocessedMessages.Clear();
		Reconnect();
	}

	public void Visit(PlayerInterpolation playerInterpolation)
	{
		var msgDrivenPlayer = MessageDrivenPlayer.FromInterpolation(playerInterpolation, MapLayer);
		msgDrivenPlayer.Player.MouseClicked += OnEntityClicked;
		if (_entityManager.Add(msgDrivenPlayer))
		{
			AddChild(msgDrivenPlayer.Player);
			LOGGER.Debug("Added player {0}.", msgDrivenPlayer.Id);
		}
	}

	public void Visit(IPredictableResponse response)
	{
		if (!_predictionManager.Reconcile(response))
		{
			_character?.Rewind(response);
		}
	}

	private void OnDragItemEvent(DragInventorySlotEvent slotEvent)
	{
		if (_character == null || _uiController == null)
		{
			return;
		}
		var globalMousePosition = GetGlobalMousePosition();
		var player = _entityManager.SelectFirst<MessageDrivenPlayer>(player => player.Id != _character.Id && player.HasPoint(globalMousePosition));
		if (player != null)
		{
			_uiController.DropItemOnPlayer(_character, player, slotEvent.Slot);
		}
		else
		{
			_uiController.DragItem(slotEvent, globalMousePosition, _character);
		}
	}

	public void Visit(ICharacterMessage characterMessage)
	{
		_character?.Handle(characterMessage);
	}

	public void Visit(RewindMessage rewindMessage)
	{
		_predictionManager.Clear();
		Visit((IEntityMessage)rewindMessage);
	}

	public void Visit(IEntityMessage message)
	{
		_entityManager.Get(message.Id)?.Handle(message);
	}

	public void Visit(TextMessage message)
	{
		_uiController?.DisplayTextMessage(message);
	}

	public void Visit(NpcInterpolation npcInterpolation)
	{
		var npc = _entityFactory.CreateNpc(npcInterpolation, MapLayer);
		npc.MouseClicked += OnEntityClicked;
		if (_entityManager.Add(npc))
		{
			AddChild(npc);
		}
	}

	public void Visit(TeleportInterpolation interpolation)
	{
		var entity = _entityFactory.CreateTeleport(interpolation);
		if (_entityManager.Add(entity))
		{
			AddChild(entity);
		}
	}

	private void Remove(IEntity? entity, RemoveEntityMessage message)
	{
		entity?.Handle(message);
		if (entity is Node2D node2D)
		{
			RemoveChild(node2D);
			LOGGER.Debug("Removed entity {0}.", entity.Id);
		}
		if (entity != null)
			_entityManager.Remove(entity.Id);
	}

	public void Visit(RemoveEntityMessage removeEntityMessage)
	{
		if (_character != null && removeEntityMessage.Id == _character.Id)
		{
			LOGGER.Debug("Removing char {0}.", _character.Id);
			var all = _entityManager.Select<IEntity>(_ => true);
			foreach (var entity in all)
			{
				Remove(entity, removeEntityMessage);
			}
			LOGGER.Debug("Removed char {0}.", _character.Id);
		}
		else
		{
			var removedEntity = _entityManager.Get(removeEntityMessage.Id);
			Remove(removedEntity, removeEntityMessage);
		}
	}

	public void Visit(ShowItemMessage message)
	{
		var groundedItem = _entityFactory.CreateOnGroundItem(message);
		_entityManager.Add(groundedItem);
		AddChild(groundedItem);
	}

	public void Visit(DynamicObjectInterpolation message)
	{
		var gameDynamicObject = _entityFactory.CreateObject(message, MapLayer);
		gameDynamicObject.BindCharacter(_character, _eventMediator);
		gameDynamicObject.MouseClicked += OnEntityClicked;
		if (_entityManager.Add(gameDynamicObject))
		{
			AddChild(gameDynamicObject);
			LOGGER.Debug("Added object {0}.", gameDynamicObject.Id);
		}
	}


	public void Visit(ProjectileMessage message)
	{
		var shooter = _entityManager.Get<ICreature>(message.ShooterId);
		if (shooter == null)
		{
			return;
		}
		var entity = _entityManager.Get(message.TargetId);
		if (entity is not ICreature creature)
		{
			return;
		}
		AddChild(Projectile.LoadFor(shooter, creature, message.SpriteId));
	}

	public void Visit(OpenBankMessage message)
	{
		var bank = CharacterBank.Create(_itemFactory, message);
		_uiController?.OpenBank(bank, _character.Inventory);
	}


	public void Visit(OpenTradeWindowMessage message)
	{
		var player = _entityManager.SelectFirst<IPlayer>(p => p.Id.Equals(message.TargetId));
		if (player == null || _character == null)
		{
			return;
		}
		_uiController?.OpenTrade(_character, player.EntityName, message.SlotId);
	}
	
	public void Visit(UpdateTradeWindowMessage message)
	{
		_uiController?.UpdateTrade(message);
	}

	public void Visit(NpcPositionMessage message)
	{
		_uiController?.DrawNpc(message);
	}

	public void Visit(PlayerChatMessage message)
	{
		_uiController?.DisplayTextMessage(new TextMessage(message.Content, TextMessage.TextLocation.DOWN));
		Visit((IEntityMessage)message);
	}

	public void Visit(BankOperationMessage message)
	{
		_uiController?.OperateBank(message);
	}

	public void Visit(JoinedRealmMessage message)
	{
		_character = CharacterImpl.LoggedIn(message, MapLayer, _itemFactory, _eventMediator);
		_character.WhenCharacterUpdated += OnCharacterEvent;
		_character.WrappedPlayer().MouseClicked += OnEntityClicked;
		_autoFillAssistant = AutoFillAssistant.Create(_character);
		_autoLootAssistant = AutoLootAssistant.Create(_entityManager, _character);
		_audioManager?.Restore(_character, message.Bgm);
		_uiController?.BindCharacter(_character, message.RealmName, _autoFillAssistant, _audioManager, _autoLootAssistant);
		MapLayer.BindCharacter(_character, message.MapName, message.TileName, message.ObjName, message.RoofName);
		_entityManager.Add(_character);
		AddChild(_character);
	}
	
	public override void _Notification(int what)
	{
		if (what == NotificationWMCloseRequest)
		{
			_channel?.WriteAndFlushAsync(ClientSimpleCommandEvent.Quit).Wait();
			_channel?.CloseAsync().Wait();
			GetTree().Quit(); 
		}
	}
}
