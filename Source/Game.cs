using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
using y1000.Source.Util;

namespace y1000.Source;

public partial class Game : Node2D, IConnectionEventListener, IServerMessageVisitor
{
	private readonly Bootstrap _bootstrap = new();

	private readonly ConcurrentQueue<object> _unprocessedMessages = new();

	private readonly Dictionary<long, IEntity> _entities = new();
	
	private readonly InputSampler _inputSampler = new();

	private readonly PredictionManager _predictionManager = new();

	private CharacterImpl? _character;

	private volatile IChannel? _channel;

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	private UIController? _uiController;

	private readonly CreatureFactory _creatureFactory = new ();

	private readonly ItemFactory _itemFactory = ItemFactory.Instance;

	private MessageFactory _messageFactory;

	private readonly EventMediator _eventMediator = new();

	public override void _Ready()
	{
		_messageFactory = new MessageFactory(_itemFactory);
		SetupNetwork();
		_uiController = GetNode<UIController>("UILayer");
		_uiController.InitEventMediator(_eventMediator);
		_eventMediator.SetComponent(WriteMessage);
		_eventMediator.SetComponent(OnDragItemEvent);
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


	private void ShowCharacter(JoinedRealmMessage joinedRealmMessage)
	{
		// character = GetNode<y1000.code.character.OldCharacter>("Character");
		// character.Coordinate = new Point(loginMessage.Coordinate.X, loginMessage.Coordinate.Y);
		// character.ChestArmor = new ChestArmor(true, "男子黄金铠甲", "T5");
		// character.Hat = new Hat(0L, "v16", "男子雨中客雨帽", true);
		// character.Trousers = new Trousers(0L, "R1", "男子长裤", true);
		// character.Weapon = new Sword(0, "W68", "耀阳宝剑");
		// character.Visible = true;
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


	private void HandleMouseInput(InputEventMouse eventMouse)
	{
		HandleCharacterInput((ch) =>
		{
			var mousePos = ch.WrappedPlayer().GetLocalMousePosition();
			return _inputSampler.SampleMoveInput(eventMouse, mousePos);
		});
	}

	private void HandleCharacterInput(Func<CharacterImpl, IPredictableInput?> sampleFunction)
	{
		if (_character == null)
		{
			return;
		}
		var input = sampleFunction.Invoke(_character);
		if (input == null)
		{
			return;
		}
		_character.HandleInput(input);
	}
	
	
	private void HandleKeyInput(InputEventKey eventKey)
	{
		if (_character == null)
		{
			return;
		}

		//_uiController?.DisplayMessage(eventKey.Keycode.ToString());
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouse eventMouse)
		{
			HandleMouseInput(eventMouse);
		}
		else if (@event is InputEventKey eventKey)
		{
			HandleKeyInput(eventKey);
		}
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
		if (sender is not IPlayer player)
		{
			return;
		}
		if (!_entities.TryGetValue(args.TargetId, out var entity))
		{
			return;
		}
		if (entity is not ICreature creature)
		{
			return;
		}
		AddChild(Projectile.Arrow(player, creature));
	}

	public void Visit(PlayerInterpolation playerInterpolation)
	{
		var msgDrivenPlayer = MessageDrivenPlayer.FromInterpolation(playerInterpolation, MapLayer, _itemFactory);
		msgDrivenPlayer.Player.OnPlayerShoot += OnPlayerShoot;
		_entities.TryAdd(msgDrivenPlayer.Id, msgDrivenPlayer);
		AddChild(msgDrivenPlayer.Player);
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
		if (_entities.TryGetValue(message.Id, out var entity))
		{
			entity.Handle(message);
		}
	}

	public void Visit(CreatureInterpolation creatureInterpolation)
	{
		var monster = Monster.Create(creatureInterpolation, MapLayer);
		monster.MouseClicked += OnCreatureClicked;
		_entities.TryAdd(monster.Id, monster);
		LOGGER.Debug("Received creature message {0}.", monster);
		AddChild(monster);
		LOGGER.Debug("Added creature {0}.", monster);
	}

	public void Visit(RemoveEntityMessage removeEntityMessage)
	{
		if (_entities.TryGetValue(removeEntityMessage.Id, out var entity))
		{
			entity.Handle(removeEntityMessage);
			_entities.Remove(removeEntityMessage.Id);
			LOGGER.Debug("Removed creature {0}.", removeEntityMessage.Id);
		}
	}

	private bool MouseWithinCharacterDropRange()
	{
		if (_character == null)
		{
			return false;
		}
		var start = _character.Coordinate.Move(-2, -3).ToPosition();
		var end = _character.Coordinate.Move(2, 3).ToPosition();
		var globalMousePosition = GetGlobalMousePosition();
		return start.X <= globalMousePosition.X && end.X >= globalMousePosition.X && 
		       start.Y <= globalMousePosition.Y && end.Y >= globalMousePosition.Y;
	}


	public void Visit(JoinedRealmMessage joinedRealmMessage)
	{
		_character = CharacterImpl.LoggedIn(joinedRealmMessage, MapLayer, _itemFactory);
		_character.WhenCharacterUpdated += OnCharacterEvent;
		_character.Inventory.SetEventMediator(_eventMediator);
		_uiController?.BindCharacter(_character);
		MapLayer.BindCharacter(_character);
		_character.WrappedPlayer().OnPlayerShoot += OnPlayerShoot;
		_entities.TryAdd(_character.Id, _character);
		AddChild(_character);
	}
	
}
