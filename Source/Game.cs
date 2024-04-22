using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Godot;
using NLog;
using y1000.code.networking.message;
using y1000.Source.Character.Event;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Control.Bottom;
using y1000.Source.Creature;
using y1000.Source.Creature.Monster;
using y1000.Source.Input;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Connection;
using y1000.Source.Player;

namespace y1000.Source;

public partial class Game : Node2D, IConnectionEventListener, IServerMessageHandler
{

	private readonly Bootstrap _bootstrap = new();

	private readonly ConcurrentQueue<object> _unprocessedMessages = new();

	private readonly Dictionary<long, ICreature> _creatures = new();
	
	private readonly Dictionary<long, IPlayer> _players = new();

	private readonly InputSampler _inputSampler = new();

	private readonly PredictionManager _predictionManager = new();

	private ConnectionState _connectionState = ConnectionState.DISCONNECTED;

	private Character.Character? _character;

	private volatile IChannel? _channel;

	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

	private BottomControl? _bottomControl;


	private enum ConnectionState
	{
		DISCONNECTED,
		CONNECTING,
		CONNECTED,
	}



	public override void _Ready()
	{
		SetupNetwork();
		_bottomControl = GetNode<BottomControl>("UILayer/BottomUI");
	}

	private void WhenCharacterUpdated(object? sender, AbstractCharacterEventArgs characterEventArgs)
	{
		if (characterEventArgs is CharacterStateEventArgs stateEventArgs)
		{
			_predictionManager.Save(stateEventArgs.Prediction);
			WriteMessage(stateEventArgs.Event);
		}
	}

	private void ShowCharacter(LoginMessage loginMessage)
	{
		// character = GetNode<y1000.code.character.OldCharacter>("Character");
		// character.Coordinate = new Point(loginMessage.Coordinate.X, loginMessage.Coordinate.Y);
		// character.ChestArmor = new ChestArmor(true, "男子黄金铠甲", "T5");
		// character.Hat = new Hat(0L, "v16", "男子雨中客雨帽", true);
		// character.Trousers = new Trousers(0L, "R1", "男子长裤", true);
		// character.Weapon = new Sword(0, "W68", "耀阳宝剑");
		// character.Visible = true;
	}

	private void AddCreature(AbstractCreature creature)
	{
		AddChild(creature);
		_creatures.Add(creature.Id, creature);
	}
	

	public bool CanMove(Point coordinate)
	{
		MapLayer mapLayer = MapLayer;
		if (mapLayer.Map == null)
		{
			return false;
		}
		if (!mapLayer.Map.IsMovable(coordinate))
		{
			return false;
		}
		return true;
		//return !_creatures.Values.Any(c => c.Coordinate.Equals(coordinate));
	}

	private async void SetupNetwork()
	{
		_bootstrap.Group(new MultithreadEventLoopGroup())
			.Handler(new ActionChannelInitializer<ISocketChannel>(c => c.Pipeline.AddLast(new LengthFieldPrepender(4), new MessageEncoder(), new LengthBasedPacketDecoder(), new MessageHandler(this))))
			.Channel<TcpSocketChannel>();
		_channel = await _bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));
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
		if (_character != null)
		{
			var mousePos = _character.WrappedPlayer().GetLocalMousePosition();
			var input = _inputSampler.Sample(eventMouse, mousePos);
			if (input == null) 
			{
				return;
			}
			if (_character.CanHandle(input))
			{
				_character.HandleInput(input);
			}
		}
	}
	

	private void HandleKeyInput(InputEventKey eventKey)
	{
	}

	public override void _Input(InputEvent @event)
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


	private async void ConnectServer()
	{
		if (_connectionState != ConnectionState.DISCONNECTED)
		{
			return;
		}
		if (_channel != null)
		{
			_connectionState = ConnectionState.CONNECTING;
			await _channel.WriteAndFlushAsync("Connect");
			_connectionState = ConnectionState.CONNECTED;
		}
	}


	public override void _Process(double delta)
	{
		HandleMessages();
	}


	private void HandleMessages()
	{
		if (!_unprocessedMessages.TryDequeue(out var message))
			return;
		if (message is IServerMessage serverMessage)
		{
			serverMessage.Accept(this);
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

	public void Handle(PlayerInterpolation playerInterpolation)
	{
		var player = Player.Player.FromInterpolation(playerInterpolation, MapLayer);
		LOGGER.Debug("Adding player {0}", player.Location());
		_players.TryAdd(player.Id, player);
		AddChild(player);
	}

	public void Handle(InputResponseMessage inputResponseMessage)
	{
		if (!_predictionManager.Reconcile(inputResponseMessage))
		{
			_character?.Rewind(inputResponseMessage.PositionMessage);
		}
	}

	public void Handle(IEntityMessage message)
	{
		if (_players.TryGetValue(message.Id, out var player))
		{
			player.Handle(message);
		}
		else if (_creatures.TryGetValue(message.Id, out var monster))
		{
			monster.Handle(message);
		}
	}

	public void Handle(CreatureInterpolation creatureInterpolation)
	{
		LOGGER.Debug("Adding creature by interpolation {0}.", creatureInterpolation);
		var monster = Monster.Create(creatureInterpolation, MapLayer);
		_creatures.TryAdd(monster.Id, monster);
		AddChild(monster);
	}

	public void Handle(LoginMessage loginMessage)
	{
		if (MapLayer.Map != null)
		{
			_character = Character.Character.LoggedIn(loginMessage, MapLayer);
			_character.WhenCharacterUpdated += WhenCharacterUpdated;
			_bottomControl?.BindCharacter(_character);
			MapLayer.BindCharacter(_character);
			AddChild(_character);
		}
	}
}
