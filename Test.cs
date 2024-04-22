using System.Net;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Godot;
using y1000.code.networking;
using y1000.Source.Networking.Connection;

namespace y1000;

public partial class Test : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private Bootstrap bootstrap = new();

	private IChannel channel;

	public override void _Ready()
	{
		SetupNetwork();
	}


	private async void SetupNetwork()
	{
		bootstrap.Group(new MultithreadEventLoopGroup())
			.Handler(new ActionChannelInitializer<ISocketChannel>(channel => channel.Pipeline.AddLast(new LengthFieldPrepender(4), new MessageEncoder(), new LengthBasedPacketDecoder())))
			.Channel<TcpSocketChannel>();
		channel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey inputEventKey) {
			if (inputEventKey.IsPressed() && inputEventKey.Keycode == Key.A) {
				//channel?.WriteAndFlushAsync(new MoveMessage(y1000.code.Direction.DOWN, new System.Drawing.Point(10, 10), 0, 0));
			}
		}
		if (@event is InputEventMouseButton button)
		{
			if (button.ButtonIndex == MouseButton.Left && button.IsPressed())
			{
				GD.Print(GetLocalMousePosition());
				GD.Print(GetLocalMousePosition().Angle());
			}
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}