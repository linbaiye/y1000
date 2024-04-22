using Godot;
using y1000.code.player;

namespace y1000.Source.Creature.State;

public interface ICreatureState<in TC> where TC : ICreature
{
    OffsetTexture BodyOffsetTexture(TC c);

    void Update(TC c, long delta);
}