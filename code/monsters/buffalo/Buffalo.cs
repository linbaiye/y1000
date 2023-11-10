using Godot;
using System;
using y1000.code;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.monsters;
using y1000.code.player;

public partial class Buffalo : AbstractCreature
{

	private SpriteContainer? spriteContainer;

	private void Setup(SpriteContainer spriteContainer) 
	{
		Setup();
		this.spriteContainer = spriteContainer;
		ChangeState(new SimpleCreatureIdleState(this, Direction.DOWN));
		CurrentState.PlayAnimation();
	}

	public static Buffalo Load()
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://monster.tcsn");
		Buffalo buffalo = scene.Instantiate<Buffalo>();
		SpriteContainer spriteContainer = SpriteContainer.Load(MonsterNames.BUFFALO);
		buffalo.Setup(spriteContainer);
		return buffalo;
	}

    public override void _Process(double delta)
    {
		base._Process(delta);
		PositionedTexture texture = BodyTexture;
		GetNode<TextureRect>("Hover").Position = new (texture.Offset.X, 0);
    }

    protected override SpriteContainer GetSpriteContainer()
    {
		if (spriteContainer == null)
		{
			throw new NotSupportedException();
		}
		return spriteContainer;
    }
}

