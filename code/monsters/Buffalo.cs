using Godot;
using System;
using y1000.code;
using y1000.code.creatures;
using y1000.code.player;

public partial class Buffalo : StaticBody2D, ICreature
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = 0;

	private Vector2 velocity = Vector2.Zero;


	private Label label = new Label
	{
		Text = "牛",
		Name = "text",
		Position =  new Vector2(4, 8)
	};



    public PositionedTexture BodyTexture => throw new NotImplementedException();

    public State State => throw new NotImplementedException();

    public Direction Direction => throw new NotImplementedException();


	public void OnAnimationFinised(StringName name)
	{
		velocity = Vector2.Zero;
		GetNode<AnimationPlayer>("AnimationPlayer").Play("AttackUp");
	}

    public override void _Ready()
    {
		Position = Position.Snapped(new Vector2(32, 24));
		velocity = new Vector2(0, -24);
		GetNode<AnimationPlayer>("AnimationPlayer").AnimationFinished += OnAnimationFinised;
    }


    public override void _PhysicsProcess(double delta)
    {
		if (velocity != Vector2.Zero)
			MoveAndCollide(velocity * (float)delta);
    }


    public void OnMouseExited()
	{
		RemoveChild(label);
	}


	public void OnMouseEntered()
	{
		AddChild(label);
    }

    public void ChangeState(IPlayerState newState)
    {

    }

}
