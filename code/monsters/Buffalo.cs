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
	public float gravity = 0;

	private Vector2 velocity = Vector2.Zero;

	private Label label = new Label
	{
		Name = "text",
		Position =  new Vector2(4, 8)
	};

	private State state;

	private AnimationPlayer animationPlayer;

	private SpriteContainer spriteContainer;


    public PositionedTexture BodyTexture
	{
		get
		{
			int offset = CurrentState.GetSpriteOffset();
			int nr = GetMeta("spriteNumber").AsInt32();
			return spriteContainer.Get(nr + offset);
		}
	}

	public void OnAnimationFinised(StringName name)
	{
		velocity = Vector2.Zero;
	}

	public override void _Ready()
	{
		Position = Position.Snapped(VectorUtil.TILE_SIZE);
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.AnimationFinished += OnAnimationFinised;
		spriteContainer = SpriteContainer.LoadSprites("buffalo");
		ChangeState(new BuffaloIdleState(this, Direction.DOWN));
	}


	public SpriteContainer SpriteContainer => spriteContainer;

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



	public AnimationPlayer AnimationPlayer => animationPlayer;

    public override PositionedTexture BodyTexture => throw new NotImplementedException();

    public void AddAnimationLibrary(string name, Func<AnimationLibrary> supplier)
	{
		if (!animationPlayer.HasAnimationLibrary(name))
		{
			animationPlayer.AddAnimationLibrary(name, supplier.Invoke());
		}
	}

	public static Buffalo Load()
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://Buffalo.tcsn");
		Buffalo buffalo = scene.Instantiate<Buffalo>();
		buffalo._Ready();
		return buffalo;
	}
}
