using Godot;
using NLog;

namespace y1000.Source;

public partial class LogInit : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NLog.LogManager.Setup().LoadConfiguration(builder => {
			builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToConsole();
		});
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}