using Godot;
using y1000.Source.Animation;

namespace y1000.Source.Creature.State;

public interface ICreatureState<in TC> where TC : ICreature
{
	OffsetTexture BodyOffsetTexture(TC player);

	void Update(TC c, int delta);
	
	CreatureState State { get; }
}
