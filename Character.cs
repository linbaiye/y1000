using Godot;
using System;
using System.Collections.Generic;
using System.Runtime;
using test.cide;

public partial class Character : CharacterBody2D, ICharacterState
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = 0f;
    //public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private State state;

	private Direction direction;

	private float speed = 32;

	private float speedX = 54;

	private float speedY = 40;

    public override void _Ready()
    {
        base._Ready();
		var all = GetChildren();
		foreach (var v in all) {
			if (v is AbstractBodyPart bodyPart)
				bodyPart.SetIndexGetter(() => (int)GetMeta("picNumber"));
		}
		CreateAnimations();
		state = State.IDLE;
		direction = Direction.DOWN;
		PlayAnimation();
	}


	private void PlayAnimation() {
		var player = GetNode<AnimationPlayer>("AnimationPlayer");
		var name = state + "/" + direction; 
		player.Play(name);
	}


	public void OnAnimationFinished(StringName anmationName) {
		SetMeta("picNumber", 0);
		state = State.IDLE;
		Velocity = Vector2.Zero;
		PlayAnimation();
	}


	private Direction ComputeDirection()
	{
		var clickPosition = GetLocalMousePosition();
		var angle = Mathf.Snapped(clickPosition.Angle(), Mathf.Pi / 4) / (Mathf.Pi / 4);
		int dir = Mathf.Wrap((int)angle, 0, 8);
        return dir switch
        {
            0 => Direction.RIGHT,
            1 => Direction.DOWN_RIGHT,
            2 => Direction.DOWN,
            3 => Direction.DOWN_LEFT,
            4 => Direction.LEFT,
            5 => Direction.UP_LEFT,
            6 => Direction.UP,
            7 => Direction.UP_RIGHT,
            _ => throw new NotSupportedException(),
        };
    }


	private Vector2 ComputeVelocity(Direction direction, Vector2 normolized)
	{
		return direction switch
		{
			Direction.UP => new Vector2(0, -40),
			Direction.DOWN => new Vector2(0, 40),
			Direction.RIGHT => new Vector2(54, 0),
			Direction.LEFT => new Vector2(-54, 0),
			Direction.DOWN_LEFT => new Vector2(-54, 40),
			Direction.DOWN_RIGHT => new Vector2(54, 40),
			Direction.UP_LEFT => new Vector2(-54, -40),
			Direction.UP_RIGHT => new Vector2(54, -40),
			_ => Vector2.Zero,
		};
	}


	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("ui_left"))
		{
			direction = Direction.LEFT;
		}
		else if (Input.IsActionJustPressed("ui_right"))
		{
			direction = Direction.RIGHT;
		}
		else if (Input.IsActionJustPressed("ui_up"))
		{
			direction = Direction.UP;
		}
		else if (Input.IsActionJustPressed("ui_down"))
		{
			direction = Direction.DOWN;
		}
		if (Input.IsActionPressed("mouse_right") && GetLocalMousePosition().Length() > 10)
		{
			var player = GetNode<AnimationPlayer>("AnimationPlayer");
			player.GetAnimation(player.CurrentAnimation).LoopMode = Animation.LoopModeEnum.Linear;
			direction = ComputeDirection();
			state = State.WALK;
			Velocity = ComputeVelocity(direction, GetLocalMousePosition().Normalized());
			PlayAnimation();
		}
		else if (Input.IsActionJustReleased("mouse_right"))
		{
			var player = GetNode<AnimationPlayer>("AnimationPlayer");
			player.GetAnimation(player.CurrentAnimation).LoopMode = Animation.LoopModeEnum.None;
		}
		MoveAndSlide();
	}

	private Animation CreateAnimation(int total, float step)
	{
		Animation animation = new Animation();
		int trackIdx = animation.AddTrack(Animation.TrackType.Value);
		animation.TrackSetPath(trackIdx, ".:metadata/picNumber");
		float time = 0.0f;
		//animation.Step = step;
		for (int i = 0; i <= total; i++, time += step)
		{
			animation.TrackInsertKey(trackIdx, time, Math.Min(i, total - 1));
		}
		animation.Length = total * step;
		return animation;
	}



	private AnimationLibrary CreateAnimations(int total, float step)
	{
		AnimationLibrary library = new AnimationLibrary();
		foreach (Direction direction in Enum.GetValues(typeof(Direction)))
		{
			library.AddAnimation(direction.ToString(), CreateAnimation(total, step));
		}
		return library;
	}


	private class AnimationConfig
	{
		public AnimationConfig(int total, float step)
		{
			Total = total;
			Step = step;
		}

		public int Total { get; set; }
		public float Step { get; set; }
	}


	private static readonly Dictionary<State, AnimationConfig> ANIMATION_CONFIG = new Dictionary<State, AnimationConfig>() {
		{State.WALK, new AnimationConfig(6, 0.1f)},
		{State.IDLE, new AnimationConfig(3, 0.5f)},
	};

	public State State => state;

	public Direction Direction => direction;

	private void CreateAnimations()
	{
		var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		foreach (State state in Enum.GetValues(typeof(State)))
		{
			if (ANIMATION_CONFIG.TryGetValue(state, out AnimationConfig animationConfig))
			{
				var library = CreateAnimations(animationConfig.Total, animationConfig.Step);
				animationPlayer.AddAnimationLibrary(state.ToString(), library);
			}
		}
	}
}
