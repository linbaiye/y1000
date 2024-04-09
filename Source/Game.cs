using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Godot;
using NLog;
using y1000.code.creatures;
using y1000.code.networking;
using y1000.code.networking.message;
using y1000.Source.Character.Event;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;
using y1000.Source.Networking;
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


	private enum ConnectionState
	{
		DISCONNECTED,
		CONNECTING,
		CONNECTED,
	}



	public override void _Ready()
	{
		//GD.Print("Loading");
		//AddChild(Buffalo.Load(new Point(38, 35)));
		//GD.Print("Loaded");
		SetupNetwork();
		var map = WorldMap.Map;
		/*if (map != null)
	{
		int n = 0;
		map.ForeachCell((cell, x, y) => {
			if (x == 36 || y == 31 || n >= 10)
			{
				return;
			}
			if (!map.IsMovable(x, y))
			{
				return;
			}
			if (x > 27 && y > 27 && x < 51 && y < 45) {
				AddCreature(SimpleCreature.Load(new Point(x, y), ++n, (Direction)new Random().Next(0, 7)));
			}
		});
	}*/
		//OtherPlayer otherPlayer = OtherPlayer.Test();
		//otherPlayer.Position = new Vector2(1248, 696);
		//AddChild(otherPlayer);
		//AddCharacter();
	}


	private void AddCharacter()
	{
		AddCharacter(new LoginMessage() { Coordinate = new Vector2I(38, 35) });
	}

	private void AddCharacter(LoginMessage loginMessage)
	{
		if (WorldMap.Map != null)
		{
			_character = Character.Character.LoggedIn(loginMessage, WorldMap.Map);
			_character.OnCharacterUpdated += OnCharacterUpdated;
			AddChild(_character);
		}
	}

	private void OnCharacterUpdated(object? sender, EventArgs args)
	{
		if (args is not CharacterUpdatedEventArgs eventArgs) return;
		_predictionManager.Save(eventArgs.Prediction);
		_channel?.WriteAndFlushAsync(eventArgs.Event);
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
		UpdateCoordinate();
	}

	private void AddCreature(AbstractCreature creature)
	{
		AddChild(creature);
		_creatures.Add(creature.Id, creature);
	}
	

	public bool CanMove(Point coordinate)
	{
		code.world.WorldMap worldMap = WorldMap;
		if (worldMap.Map == null)
		{
			return false;
		}
		if (!worldMap.Map.IsMovable(coordinate))
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

	private async void WriteMessage(I2ServerGameMessage message)
	{
		await Task.Run(() =>
		{
			Thread.Sleep(200);
		});
		_channel?.WriteAndFlushAsync(message);
	}


	public void SendMessage(I2ServerGameMessage message)
	{
		WriteMessage(message);
	}

	private code.world.WorldMap WorldMap => GetNode<code.world.WorldMap>("MapLayer");


	private void HandleMouseInput(InputEventMouse eventMouse)
	{
		var worldMap = GetNode<code.world.WorldMap>("MapLayer");
		if (worldMap is { Map: not null } && _channel != null && _character != null)
		{
			var mousePos = _character.GetLocalMousePosition();
			var input = _inputSampler.Sample(eventMouse, mousePos);
			if (input == null) 
			{
				return;
			}
			if (_character.CanHandle(input))
			{
				_character.HandleInput(input);
			}
			//_predictionManager.Save(_character.Predict(input));
			//logger.Debug("Sending input {0}.", input);
			//channel.WriteAndFlushAsync(input);
		}
		if (eventMouse is InputEventMouseButton button)
		{
			if (button.ButtonIndex == MouseButton.Right)
			{
				if (button.IsPressed())
				{
					//character?.Move(GetLocalMousePosition());
				}
				else if (button.IsReleased())
				{
					//character?.StopMove();
				}
			}
			else if (button.ButtonIndex == MouseButton.Left)
			{
				if (button.IsPressed() && button.DoubleClick)
				{
					var children = GetChildren();
					foreach (var child in children)
					{
						if (child is AbstractCreature creature)
						{
						}
					}
				}
			}
		}
		else if (eventMouse is InputEventMouseMotion mouseMotion)
		{
			if ((mouseMotion.ButtonMask & MouseButtonMask.Right) != 0)
			{
				//character?.Move(GetLocalMousePosition());
			}
		}
	}

	private void HandleKeyInput(InputEventKey eventKey)
	{
		if (eventKey.Keycode != Key.Shift && eventKey.Keycode != Key.Ctrl)
		{
			if (eventKey.Keycode == Key.F3)
			{
				//character?.Sit();
			}
			else if (eventKey.Keycode == Key.H)
			{
				//character?.Hurt();
			}
			else if (eventKey.Keycode == Key.F6)
			{
				//if (eventKey.IsPressed())
				//	character?.PressBufa(new UnnamedBufa());
			}
		}
		/*var monster = GetNode<SimpleCreature>("Monster");
	if (eventKey.IsPressed())
	{
		switch (eventKey.Keycode)
		{
			case Key.Left:
				monster.Move(Direction.LEFT);
				break;
			case Key.Right:
				monster.Move(Direction.RIGHT);
				break;
			case Key.Down:
				monster.Move(Direction.DOWN);
				break;
			case Key.Up:
				monster.Move(Direction.UP);
				break;
			case Key.I:
				monster.Move(Direction.UP_RIGHT);
				break;
			case Key.J:
				monster.Move(Direction.DOWN_RIGHT);
				break;
			case Key.K:
				monster.Move(Direction.DOWN_LEFT);
				break;
			case Key.L:
				monster.Move(Direction.UP_LEFT);
				break;
		}
	}*/
	}

	public override void _Input(InputEvent @event)
	{
		//inputSampler.Sample(@event, GetLocalMousePosition());
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
		UpdateCoordinate();
	}


	// private void Handle(InterpolationsMessage message)
	// {
	// 	foreach (IInterpolation interpolation in message.Interpolations)
	// 	{
	// 		if (otherPlayers.TryGetValue(interpolation.Id, out OtherPlayer? other))
	// 		{
	// 			other.EnqueueInterpolation(interpolation);
	// 		}
	// 		else
	// 		{
	// 			OtherPlayer player = OtherPlayer.CreatePlayer(interpolation);
	// 			otherPlayers.Add(interpolation.Id, player);
	// 			AddChild(player);
	// 		}
	// 	}
	// }
	
	private void HandleMessages()
	{
		if (_unprocessedMessages.IsEmpty)
		{
			return;
		}
		if (_unprocessedMessages.TryDequeue(out var message))
		{
			if (message is IServerMessage serverMessage)
			{
				serverMessage.Accept(this);
			}
		}
	}


	private void UpdateCoordinate()
	{
		var coor = _character?.Coordinate;
		if (coor == null)
		{
			return;
		}
		var label = GetNode<Label>("UILayer/BottomUI/Container/Control/Skill/Coordinate/Label");
		if (label != null)
		{
			label.Text = coor.ToString();
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
		var player = Player.Player.FromInterpolation(playerInterpolation);
		LOGGER.Debug("Adding player {0}", player.Location());
		_players.TryAdd(player.Id, player);
		AddChild(player);
	}

	public void Handle(InputResponseMessage inputResponseMessage)
	{
		if (!_predictionManager.Reconcile(inputResponseMessage))
		{
			LOGGER.Debug("Need to rewind.");
			_character?.Rewind(inputResponseMessage.PositionMessage);
		}
	}

	public void Handle(IEntityMessage message)
	{
		if (_players.TryGetValue(message.Id, out var player))
		{
			player.Handle(message);
		}
	}


	public void Handle(LoginMessage loginMessage)
	{
		AddCharacter(loginMessage);
	}
}
