using DotNetty.Codecs;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Godot;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using y1000.code;
using y1000.code.character.state.input;
using y1000.code.creatures;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;
using y1000.code.networking;
using y1000.code.networking.message;
using y1000.code.player;
using y1000.code.player.skill;
using y1000.code.player.skill.bufa;
using y1000.code.util;
using y1000.code.world;

public partial class Game : Node2D, IConnectionEventListener
{
	private y1000.code.character.Character? character;

	private volatile IChannel? channel;

	private readonly Bootstrap bootstrap = new();

	private ConcurrentQueue<string> packets = new ConcurrentQueue<string>();

	private ConcurrentQueue<IGameMessage> unprocessedMessages = new ConcurrentQueue<IGameMessage>();

	private Dictionary<long, ICreature> creatures = new Dictionary<long, ICreature>();


	private readonly InputSampler inputSampler = new InputSampler();

	private enum ConnectionState
	{
		DISCONNECTED,
		CONNECTING,
		CONNECTED,
	}



	private ConnectionState state = ConnectionState.DISCONNECTED;

	public override void _Ready()
	{
		//GD.Print("Loading");
		//AddChild(Buffalo.Load(new Point(38, 35)));
		//GD.Print("Loaded");
		//SetupNetwork();
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
		OtherPlayer otherPlayer = OtherPlayer.Test();
		otherPlayer.Position = new Vector2(1248, 696);
		AddChild(otherPlayer);
	}

	private void ShowCharacter(LoginMessage loginMessage)
	{
		character = GetNode<y1000.code.character.Character>("Character");
		character.Coordinate = loginMessage.Coordinate;
		character.ChestArmor = new ChestArmor(true, "男子黄金铠甲", "T5");
		character.Hat = new Hat(0L, "v16", "男子雨中客雨帽", true);
		character.Trousers = new Trousers(0L, "R1", "男子长裤", true);
		character.Weapon =  new Sword(0, "W68", "耀阳宝剑");
		character.Visible = true;
		UpdateCoordinate();
	}

	private void AddCreature(AbstractCreature creature)
	{
		AddChild(creature);
		creatures.Add(creature.Id, creature);
	}


	public bool CanMove(Point coordinate)
	{
		WorldMap worldMap = WorldMap;
		if (worldMap == null || worldMap.Map == null)
		{
			return false;
		}
		if (!worldMap.Map.IsMovable(coordinate))
		{
			return false;
		}
		return !creatures.Values.Any(c => c.Coordinate.Equals(coordinate));
	}

	private async void SetupNetwork()
	{
		bootstrap.Group(new MultithreadEventLoopGroup())
		.Handler(new ActionChannelInitializer<ISocketChannel>(channel => channel.Pipeline.AddLast(new LengthFieldPrepender(4), new MessageEncoder(), new LengthBasedPacketDecoder(), new MessageHandler(this))))
		.Channel<TcpSocketChannel>();
		channel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));
	}

	private async void WriteMessage(I2ServerGameMessage message)
	{
		await Task.Run(() =>
		{
			Thread.Sleep(200);
		});
		channel?.WriteAndFlushAsync(message);
	}


	public void SendMessage(I2ServerGameMessage message)
	{
		WriteMessage(message);
	}

	public WorldMap WorldMap => GetNode<WorldMap>("MapLayer");

	private void HandleMouseInput(InputEventMouse eventMouse)
	{
		var worldMap = GetNode<WorldMap>("MapLayer");
		if (worldMap != null && worldMap.Map != null)
		{
			character?.HandleInput(eventMouse);
			//character?.HandleMouseInput(worldMap.Map, creatures.Values, eventMouse);
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
				character?.Sit();
			}
			else if (eventKey.Keycode == Key.H)
			{
				character?.Hurt();
			}
			else if (eventKey.Keycode == Key.F6)
			{
				if (eventKey.IsPressed())
					character?.PressBufa(new UnnamedBufa());
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
		inputSampler.Sample(@event, GetLocalMousePosition());
		if (@event is InputEventMouse eventMouse)
		{
			HandleMouseInput(eventMouse);
		}
		else if (@event is InputEventKey eventKey)
		{
			HandleKeyInput(eventKey);
		}
	}


	public async void LogIn()
	{
		if (state != ConnectionState.DISCONNECTED)
		{
			return;
		}
		if (channel != null)
		{
			state = ConnectionState.CONNECTING;
			await channel.WriteAndFlushAsync("Connect");
			state = ConnectionState.CONNECTED;
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HandleMessages();
		UpdateCoordinate();
	}

	private void ShowPlayer(ShowPlayerMessage showPlayerMessage)
	{
		//Player player = Player.Create(showPlayerMessage);
		//AddCreature(player);
	}

	private void HandleMessages()
	{
		if (unprocessedMessages.IsEmpty)
		{
			return;
		}
		if (unprocessedMessages.TryDequeue(out IGameMessage? message))
		{
			if (message is LoginMessage loginMessage)
			{
				ShowCharacter(loginMessage);
			}
			else if (message is ShowPlayerMessage showPlayer)
			{
				ShowPlayer(showPlayer);
			}
			else
			{
				character?.HandleMessage(message);

			}
		}
	}


	private void UpdateCoordinate()
	{
		var coor = character?.Coordinate;
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

	private void HandlePackets()
	{
		if (packets.Count == 0)
		{
			return;
		}
		if (!packets.TryDequeue(out string? message))
		{
			return;
		}
		var monster = GetNode<Buffalo>("Monster");
		switch (message)
		{
			case "1":
				monster.Turn(Direction.LEFT);
				break;
			case "2":
				monster.Turn(Direction.RIGHT);
				break;
			case "3":
				monster.Turn(Direction.DOWN);
				break;
			case "4":
				monster.Turn(Direction.UP);
				break;
			case "5":
				monster.Turn(Direction.UP_RIGHT);
				break;
			case "6":
				monster.Turn(Direction.DOWN_RIGHT);
				break;
			case "7":
				monster.Turn(Direction.DOWN_LEFT);
				break;
			case "8":
				monster.Turn(Direction.UP_LEFT);
				break;
		}
	}

    public void OnMessageArrived(IGameMessage message)
    {
		unprocessedMessages.Enqueue(message);
    }

	private async void Reconnect()
	{
		await Task.Run(() => {
			Task.Delay(2000);
		});
		try
		{
			channel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));
		} catch(Exception) {
			Reconnect();
		}
	}

	public void OnConnectionClosed()
	{
		channel = null;
		unprocessedMessages.Clear();
		Reconnect();
	}
}
