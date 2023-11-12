using Godot;
using System;
using System.Drawing;
using y1000.code;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.monsters;
using y1000.code.player;

public partial class Buffalo : AbstractCreature
{

	private SpriteContainer? spriteContainer;

	private Point initCoordinate = Point.Empty;

	private long id;

	private Direction initDirection;

    private void Initiliaze(Point i, SpriteContainer spriteContainer, long id, Direction direction)
	{
		initCoordinate = i;
		this.id = id;
		this.spriteContainer = spriteContainer;
		initDirection = direction;
	}

    public override long Id => id;

    public override void _Ready()
    {
		Setup();
		ChangeState(new SimpleCreatureIdleState(this, initDirection));
		ZIndex = 2;
		YSortEnabled = true;
		ZAsRelative = true;
		Coordinate = initCoordinate;
		CurrentState.PlayAnimation();
    }


	public static Buffalo Load(Point coordinate, long id, Direction direction)
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/Monster.tscn");
		Buffalo buffalo = scene.Instantiate<Buffalo>();
		SpriteContainer spriteContainer = SpriteContainer.LoadMonsterSprites(MonsterNames.BUFFALO);
		buffalo.Initiliaze(coordinate, spriteContainer, id, direction);
		return buffalo;
	}
	public static Buffalo Load(Point coordinate, long id)
	{
		return Load(coordinate, id, Direction.DOWN);
	}

    protected override SpriteContainer GetSpriteContainer()
    {
		if (spriteContainer != null)
		{
			return spriteContainer;
		}
		throw new NotSupportedException();
    }
}

