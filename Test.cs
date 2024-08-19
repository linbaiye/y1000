using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Godot;
using NLog;
using y1000.code.networking;
using y1000.Source.Networking.Connection;
using Timer = System.Timers.Timer;

namespace y1000;

public partial class Test : Panel
{
	// Called when the node enters the scene tree for the first time.
	private Bootstrap bootstrap = new();

	private IChannel channel;
	private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

	private int _clickCount;
	internal class Sampler
	{

		private readonly Timer _timer = new(200);
		public Sampler()
		{
			_timer.Elapsed += OnTimeout;
		}

		public void SampleInput(InputEventMouseButton button)
		{
			GD.Print(Thread.CurrentThread.ManagedThreadId);
			GD.Print("Timer enabled " + _timer.Enabled);
			if (!_timer.Enabled)
			{
				_timer.Enabled = true;
			}
		}

		public void OnTimeout(object? sender, EventArgs args)
		{
			GD.Print(Thread.CurrentThread.ManagedThreadId);
		}

	}

	private Sampler _sampler = new Sampler();

	public override void _Ready()
	{
		// SetupNetwork();
	}
	internal enum MouseAction
	{
		CLICK,
		DOUBLE_CLICK,
	}
	


	private async Task<MouseAction> ParseMouseAction()
	{
		await Task.Run(() =>
		{
			return 200;
		});
		GD.Print(_clickCount);
		return _clickCount == 1 ? MouseAction.CLICK : MouseAction.DOUBLE_CLICK;
	}

	private async void SetupNetwork()
	{
		bootstrap.Group(new MultithreadEventLoopGroup())
			.Handler(new ActionChannelInitializer<ISocketChannel>(channel => channel.Pipeline.AddLast(new LengthFieldPrepender(4), new MessageEncoder() )))
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
		if (@event is not InputEventMouseButton button)
		{
			return;
		}
		GD.Print(button.GetModifiersMask());
		// _sampler.SampleInput(button);
		/*if (button.IsPressed())
			GD.Print("Press event.");
		else if (button.IsReleased())
			GD.Print("Release event");
		if (button.DoubleClick)
			GD.Print("Double click"); */
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}