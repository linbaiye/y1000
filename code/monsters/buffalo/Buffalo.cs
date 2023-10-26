using Godot;
using System;
using y1000.code;
using y1000.code.creatures;
using y1000.code.monsters;
using y1000.code.player;

public partial class Buffalo : AbstractCreature
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	private Vector2 velocity = Vector2.Zero;

	public override void _Ready()
	{
		Setup(MonsterNames.BUFFALO);
		ChangeState(new BuffaloIdleState(this, Direction.DOWN));
	}


	public static Buffalo Load()
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://Buffalo.tcsn");
		Buffalo buffalo = scene.Instantiate<Buffalo>();
		buffalo._Ready();
		return buffalo;
	}

}
