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
using y1000.Source.Character;
using y1000.Source.Character.Event;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Control;
using y1000.Source.Creature;
using y1000.Source.Creature.Monster;
using y1000.Source.DynamicObject;
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
using y1000.Source.Util;

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
	
	private AudioStreamPlayer _audioStreamPlayer;

	public Game()
	{
		_eventMediator = InitializeEventMediator();
		_itemFactory = ItemFactory.Instance;
		_messageFactory = new MessageFactory(_itemFactory);
		_entityManager = EntityManager.Instance;
		_spriteRepository = FilesystemSpriteRepository.Instance;
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

	public override void _Ready()
	{
		SetupNetwork();
		_uiController = GetNode<UIController>("UILayer");
		_uiController.Initialize(_eventMediator, _spriteRepository, _itemFactory);
		_audioStreamPlayer = GetNode<AudioStreamPlayer>("BgmPlayer");
		_audioStreamPlayer.Finished += () => _audioStreamPlayer.Play();
		//AtdChecker.Dump();
	}

	private void LoadAndPlayBackgroundMusic(string bgm)
	{
		var path = "res://assets/bgm/" + bgm + ".mp3";
		if (!FileAccess.FileExists(path))
		{
			path = "res://assets/bgm/" + bgm + ".wav";
		}
		if (FileAccess.FileExists(path))
		{
			var streamWav = ResourceLoader.Load<AudioStreamMP3>(path);
			_audioStreamPlayer.Stop();
			_audioStreamPlayer.Stream = streamWav;
			_audioStreamPlayer.Play();
		}
	}
	

	private void OnCharacterEvent(object? sender, EventArgs eventArgs)
	{
		if (sender is not CharacterImpl)
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
			var all = _entityManager.Select<AbstractEntity>(e => !e.Id.Equals(_character.Id));
			foreach (var entity in all)
			{
				RemoveChild(entity);
				_entityManager.Remove(entity.Id);
			}
			LoadAndPlayBackgroundMusic(teleportedArgs.Bgm);
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
		await _channel.WriteAndFlushAsync(new LoginEvent());
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


	private void LocalTest()
	{
		if (_character == null)
		{
			return;
		}
		var monster = _entityManager.Find<GameDynamicObject>("");
		if (monster != null)
		{
			monster.Handle(new UpdateDynamicObjectMessage(1L, 0, 4, false));
		}
	}
	

	
	private void HandleInput(InputEvent @event)
	{
		if (_character == null)
		{
			return;
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
			_uiController.TradePlayer(_character, player, slotEvent.Slot);
		}
		else
		{
			_uiController.DropItem(slotEvent, globalMousePosition, _character.Coordinate);
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
			LOGGER.Debug("Added creature {0}.", npc.Id);
		}
	}

	public void Visit(RemoveEntityMessage removeEntityMessage)
	{
		var removedEntity = _entityManager.Remove(removeEntityMessage.Id);
		removedEntity?.Handle(removeEntityMessage);
		if (removedEntity is Node2D node2D)
		{
			RemoveChild(node2D);
		}
		LOGGER.Debug("Removed creature {0}.", removeEntityMessage.Id);
	}

	public void Visit(ShowItemMessage message)
	{
		var groundedItem = _entityFactory.CreateOnGroundItem(message);
		_entityManager.Add(groundedItem);
		//LOGGER.Debug("Add item {2} at position {0}, {1}.", groundedItem.OffsetBodyPosition, groundedItem.Coordinate, groundedItem.Id);
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


	public void Visit(JoinedRealmMessage message)
	{
		_character = CharacterImpl.LoggedIn(message, MapLayer, _itemFactory, _eventMediator);
		_character.WhenCharacterUpdated += OnCharacterEvent;
		_character.WrappedPlayer().MouseClicked += OnEntityClicked;
		_uiController?.BindCharacter(_character, message.RealmName);
		MapLayer.BindCharacter(_character, message.MapName, message.TileName, message.ObjName, message.RoofName);
		_entityManager.Add(_character);
		AddChild(_character);
		LoadAndPlayBackgroundMusic(message.Bgm);
	}
}
