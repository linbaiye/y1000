using Godot;

namespace y1000.code;

public partial class Enemy : Area2D
{
	// Called when the node enters the scene tree for the first time.
	private TextureProgressBar hpBar;

	public override void _Ready()
	{
		hpBar = GetNode<TextureProgressBar>("HPBar");
		hpBar.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_right"))
		{
			hpBar.Visible = true;
			hpBar.Value = hpBar.Value - 10;
		}
		else
		{
		}
	}
}