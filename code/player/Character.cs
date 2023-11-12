using Godot;
using System;
using System.Collections.Generic;
using System.Runtime;
using y1000.code.player;
using y1000.code;
using System.Drawing;
using y1000.code.creatures;
using y1000.code.entity;
using y1000.code.world;

public partial class Character : AbstractPlayer
{
    public override long Id => throw new NotImplementedException();


    public override void _Ready()
    {
    }

}
/* Node2D, IPlayer
{
	public float gravity = 0f;

	private IPlayerState playerState;

    public override void _Ready()
    {
        base._Ready();
		AnimationPlayer.AnimationFinished += AnimationFinished;
		playerState = new IdleState(this, Direction.DOWN);
		Position = Position.Snapped(new Vector2(32, 24));
	}


	public void ChangeState(IPlayerState newState)
	{
		playerState = newState;
	}

	public CharAnimationPlayer AnimationPlayer => GetNode<CharAnimationPlayer>("AnimationPlayer");

    public int PictureNumber => (int)GetMeta("spriteNumber");

	public void ResetPictureNumber()
	{
		SetMeta("spriteNumber", 0);
	}

	public void AnimationFinished(StringName animationName)
	{
		playerState.OnAnimationFinished(animationName);
	}

    public override void _Input(InputEvent @event)
    {
		/*if (@event is InputEventMouseButton button)
		{
			if (button.ButtonIndex == MouseButton.Right)
			{
				if (button.IsPressed())
				{
					playerState.RightMousePressed(GetLocalMousePosition());
				}
				else if (button.IsReleased())
				{
					playerState.RightMouseRleased();
				}
				else
				{
					//playerState.RightMousePressed(GetLocalMousePosition());
				}
			} else if (button.ButtonIndex == MouseButton.Left)
			{
				if (button.DoubleClick)
				{
				GD.Print("received in character: " + @event.AsText());
				//GetViewport().SetInputAsHandled(); //	GD.Print("Double");
				}
			}
		}
		else if (@event is InputEventMouseMotion mouseMotion)
		{
			if ((mouseMotion.ButtonMask & MouseButtonMask.Right) != 0)
			{
				playerState.RightMousePressed(GetLocalMousePosition());
			}
		}
		else
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.IsPressed())
			{
				switch (eventKey.Keycode)
				{
					case Key.A:
						playerState.Attack();
						break;
					case Key.F3:
						playerState.Hurt();
						break;
				}
			}
		}
	}


	public PositionedTexture BodyTexture => playerState.BodyTexture;

	public override void _Process(double delta)
	{
		playerState.Process(delta);
		GetNode<TextureRect>("Hover").Position = new (BodyTexture.Offset.X, 0);
	}


	public PositionedTexture HandTexture => playerState.HandTexture;

	public State State => playerState.State;

	public Direction Direction => playerState.Direction;

	public Point Coordinate
	{
		get
		{
			return new Point((int)(Position.X / 32), (int)(Position.Y /  24));
		}
		set => throw new NotImplementedException();
	}


    public static int Add(int a, int b) => a + b;

    public void Move(Direction direction)
    {
        throw new NotImplementedException();
    }

    public void Attack()
    {
		playerState.Attack();
    }

    public void Hurt()
    {
        throw new NotImplementedException();
    }

    public void Die()
    {
        throw new NotImplementedException();
    }

    public void Move(Vector2 mousePosition)
    {
		playerState.RightMousePressed(GetLocalMousePosition());
    }

    public void StopMove()
    {
		playerState.RightMouseRleased();
    }

    public Rectangle CollisionRect()
    {
        throw new NotImplementedException();
    }

    public void Attack(ICreature target)
    {
		playerState.Attack(target);
    }

    public void Remove()
    {
		QueueFree();
    }

    public void Turn(Direction direction)
    {
        throw new NotImplementedException();
    }

    public void Sit()
    {
        throw new NotImplementedException();
    }

    public void Bow()
    {
        throw new NotImplementedException();
    }
}*/
