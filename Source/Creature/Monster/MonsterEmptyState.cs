using Godot;
using y1000.Source.Animation;

namespace y1000.Source.Creature.Monster;

public class MonsterEmptyState : IMonsterState
{
	public OffsetTexture BodyOffsetTexture(Monster player)
	{
		throw new System.NotImplementedException();
	}

	public void Update(Monster c, int delta)
	{
		throw new System.NotImplementedException();
	}

	public CreatureState State => CreatureState.AXE;
	public OffsetTexture? EffectOffsetTexture(Monster creature)
	{
		throw new System.NotImplementedException();
	}
}
