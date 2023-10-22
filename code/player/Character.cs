using Godot;
using NLog;
using System;
using System.Collections.Generic;
using System.Runtime;
using y1000.code.player;
using y1000.code;

public partial class Character : CharacterBody2D
{
	public float gravity = 0f;

	private IPlayerState playerState;

    public override void _Ready()
    {
        base._Ready();
		AnimationPlayer.AnimationFinished += AnimationFinished;
		playerState = new IdleState(this, Direction.DOWN);
	}

	public void ChangeState(IPlayerState newState)
	{
		playerState = newState;
	}

	public AnimationPlayer AnimationPlayer => GetNode<AnimationPlayer>("AnimationPlayer");

    public int PictureNumber => (int)GetMeta("picNumber");

	public void ResetPictureNumber()
	{
		SetMeta("picNumber", 0);
	}

	public void AnimationFinished(StringName animationName)
	{
		ResetPictureNumber();
		playerState.OnAnimationFinished(animationName);
	}


	public PositionedTexture BodyTexture => playerState.BodyTexture;

    public override void _PhysicsProcess(double delta)
    {
		playerState.PhysicsProcess(delta);
    }

    public State State => playerState.State;

	public Direction Direction => playerState.Direction;

}
