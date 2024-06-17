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

	public Game()
	{
		_eventMediator = InitializeEventMediator();
		_itemFactory = ItemFactory.Instance;
		_messageFactory = new MessageFactory(_itemFactory);
		_entityFactory = new EntityFactory(_eventMediator);
		_entityManager = EntityManager.Instance;
		GD.Print("Creating sprite repo.");
		_spriteRepository = FilesystemSpriteRepository.Instance;
		GD.Print("Done init.");
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
		_uiController.InitEventMediator(_eventMediator);
		GetNode<AudioStreamPlayer>("BgmPlayer").Finished += PlayBackgroundMusic;
		PlayBackgroundMusic();
	}

	private void PlayBackgroundMusic()
	{
		GetNode<AudioStreamPlayer>("BgmPlayer").Play();
	}

	private void OnCharacterEvent(object? sender, EventArgs eventArgs)
	{
		if (sender is not CharacterImpl ||
		    eventArgs is not CharacterPredictionEventArgs predictionEventArgs)
		{
			return;
		}
		_predictionManager.Save(predictionEventArgs.Prediction);
		WriteMessage(predictionEventArgs.Event);
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

	private void OnCreatureClicked(object? sender, CreatureMouseClickEventArgs args)
	{
		var click = _inputSampler.SampleLeftClickInput(args.MouseEvent, args.Creature);
		if (click is AttackInput attack)
		{
			_character?.HandleInput(attack);
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

	private void OnPlayerShoot(object? sender, PlayerRangedAttackEventArgs args)
	{
		LOGGER.Debug("Player shoot.");
		if (sender is not IPlayer player)
		{
			return;
		}
		var entity = _entityManager.Get(args.TargetId);
		LOGGER.Debug("Shoot target {0}.", args.TargetId);
		if (entity is not ICreature creature)
		{
			return;
		}
		AddChild(Projectile.Arrow(player, creature));
	}

	public void Visit(PlayerInterpolation playerInterpolation)
	{
		var msgDrivenPlayer = MessageDrivenPlayer.FromInterpolation(playerInterpolation, MapLayer);
		msgDrivenPlayer.Player.OnPlayerShoot += OnPlayerShoot;
		msgDrivenPlayer.Player.MouseClicked += OnCreatureClicked;
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
		if (_character != null)
		{
			_eventMediator.NotifyDragItemEvent(slotEvent, GetGlobalMousePosition(), _character.Coordinate);
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
		_uiController?.DisplayMessage(message.Text);
	}
		

	public void Visit(CreatureInterpolation creatureInterpolation)
	{
		var monster = Monster.Create(creatureInterpolation, MapLayer);
		monster.MouseClicked += OnCreatureClicked;
		if (_entityManager.Add(monster))
		{
			AddChild(monster);
			LOGGER.Debug("Added creature {0}.", monster.Id);
		}
	}

	public void Visit(RemoveEntityMessage removeEntityMessage)
	{
		var remove = _entityManager.Remove(removeEntityMessage.Id);
		remove?.Handle(removeEntityMessage);
		if (remove is Node2D node2D)
		{
			RemoveChild(node2D);
		}
		LOGGER.Debug("Removed creature {0}.", removeEntityMessage.Id);
	}

	public void Visit(ShowItemMessage message)
	{
		var groundedItem = _entityFactory.CreateGroundedItem(message);
		_entityManager.Add(groundedItem);
		LOGGER.Debug("Add item {2} at position {0}, {1}.", groundedItem.OffsetBodyPosition, groundedItem.Coordinate, groundedItem.Id);
		AddChild(groundedItem);
	}


	public void Visit(JoinedRealmMessage joinedRealmMessage)
	{
		_character = CharacterImpl.LoggedIn(joinedRealmMessage, MapLayer, _itemFactory, _eventMediator);
		_character.WhenCharacterUpdated += OnCharacterEvent;
		_character.WrappedPlayer().MouseClicked += OnCreatureClicked;
		_character.WrappedPlayer().OnPlayerShoot += OnPlayerShoot;
		_uiController?.BindCharacter(_character);
		MapLayer.BindCharacter(_character);
		_entityManager.Add(_character);
		AddChild(_character);
	}
}
