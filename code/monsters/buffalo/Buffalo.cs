using Godot;
using System;
using y1000.code;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.monsters;
using y1000.code.player;

public partial class Buffalo : AbstractCreature
{
	public override void _Ready()
	{
		Setup(MonsterNames.BUFFALO);
		ChangeState(new SimpleCreatureIdleState(this, Direction.DOWN));
		CurrentState.PlayAnimation();
		Position = Position.Snapped(VectorUtil.TILE_SIZE);
	}

	public static Buffalo Load()
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://monster.tcsn");
		Buffalo buffalo = scene.Instantiate<Buffalo>();
		buffalo._Ready();
		return buffalo;
	}

    public override void _Process(double delta)
    {
		base._Process(delta);
		PositionedTexture texture = BodyTexture;
		GetNode<TextureRect>("Hover").Position = new (texture.Offset.X, 0);
    }
}

