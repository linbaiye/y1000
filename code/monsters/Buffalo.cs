using Godot;
using System;

public partial class Buffalo : StaticBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = 0;


	private Label label = new Label
	{
		Text = "牛",
		Name = "text",
		Position =  new Vector2(4, 8)
	};


    public override void _PhysicsProcess(double delta)
    {
		//MoveAndCollide(new Vector2(32, 0) * (float)delta);
    }


    public void OnMouseExited()
	{
		RemoveChild(label);
	}


	public void OnMouseEntered()
	{
		AddChild(label);
    }
}
