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
	}

	public static Buffalo Load()
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://monster.tcsn");
		Buffalo buffalo = scene.Instantiate<Buffalo>();
		buffalo._Ready();
		return buffalo;
	}
}
