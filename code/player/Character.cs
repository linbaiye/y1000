using Godot;
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
		playerState.OnAnimationFinished(animationName);
	}

   public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseButton button && button.ButtonIndex == MouseButton.Right)
		{
			if (button.IsPressed())
			{
				playerState.RightMousePressed(GetLocalMousePosition());
			} else if (button.IsReleased())
			{
				playerState.RightMouseRleased();
			} else 
			{
				//playerState.RightMousePressed(GetLocalMousePosition());
			}
		}
		else if (@event is InputEventMouseMotion mouseMotion)
		{
			if ((mouseMotion.ButtonMask & MouseButtonMask.Right) != 0)
			{
				playerState.RightMousePressed(GetLocalMousePosition());
			}
		}
		else if (@event is InputEventKey eventKey)
		{
			if (eventKey.IsPressed())
			{
				switch (eventKey.Keycode)
				{
					case Key.A:
						playerState.Attack();
						break;
					case Key.F3:
						break;
				}
			}
		}

    }


    public PositionedTexture BodyTexture => playerState.BodyTexture;

    public override void _PhysicsProcess(double delta)
    {
		playerState.PhysicsProcess(delta);
    }

    public State State => playerState.State;

	public Direction Direction => playerState.Direction;


	public static int Add(int a, int b) => a + b;

}
