using Godot;
using NLog;

namespace y1000.Source.Control.LeftSide;

public partial class LeftsideTextControl : VBoxContainer
{
	private LeftsideTextLabel _label1;
	private LeftsideTextLabel _label2;
	private LeftsideTextLabel _label3;
	private LeftsideTextLabel _label4;
	private LeftsideTextLabel _label5;
	private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

	public override void _Ready()
	{
		_label1 = GetNode<LeftsideTextLabel>("Label1");
		_label2 = GetNode<LeftsideTextLabel>("Label2");
		_label3 = GetNode<LeftsideTextLabel>("Label3");
		_label4 = GetNode<LeftsideTextLabel>("Label4");
		_label5 = GetNode<LeftsideTextLabel>("Label5");
		Visible = true;
	}

	private readonly string[] _textArray = new string[5];


	public void Display(string text)
	{
		if (!_label5.Available)
		{
			Roll();
			_label5.Clean();
		}
		_label5.Display(text);
	}

	private void Roll()
	{
		LeftsideTextLabel[] labels = { _label1, _label2, _label3, _label4, _label5 };
		for (int i = 1; i < labels.Length; i++)
		{
			labels[i - 1].Copy(labels[i]);
		}
	}

	public override void _Process(double delta)
	{
		LeftsideTextLabel[] labels = { _label1, _label2, _label3, _label4, _label5 };
		for (int i = 0; i < labels.Length; i++)
		{
			var l = labels[i];
			l.Update(delta);
			if (l.Timeout)
			{
				if (i == 0)
					l.Clean();
			}
			if (l.Available)
			{
				if (i + 1 < labels.Length && labels[i + 1].Timeout)
				{
					l.Copy(labels[i + 1]);
					labels[i + 1].Clean();
				}
			}
		}
	}
}
