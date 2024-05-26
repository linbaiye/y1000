using System.Drawing;
using Godot;
using y1000.code.creatures.state;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Sprite;
using AbstractCreature = y1000.code.creatures.AbstractCreature;

namespace y1000.code.monsters.buffalo;

public partial class Buffalo : AbstractCreature
{

	private AtzSprite? spriteContainer;

	private Point initCoordinate = Point.Empty;

	private long id;

	private Direction initDirection;

	private void Initiliaze(Point i, AtzSprite atzSprite, long id, Direction direction)
	{
		initCoordinate = i;
		this.id = id;
		spriteContainer = atzSprite;
		initDirection = direction;
	}

	public override long Id => id;

	public override void _Ready()
	{
		SetupAnimationPlayer();
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
		AtzSprite atzSprite = AtzSprite.LoadOffsetMonsterSprites(MonsterNames.BUFFALO);
		buffalo.Initiliaze(coordinate, atzSprite, id, direction);
		return buffalo;
	}
	public static Buffalo Load(Point coordinate, long id)
	{
		return Load(coordinate, id, Direction.DOWN);
	}
}