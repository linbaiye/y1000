using Godot;
using y1000.Source.Creature;

namespace y1000.Source.Map;

public interface IMap
{
    bool Movable(Vector2I coordinate);

    void Occupy(ICreature creature);

    void Free(ICreature creature);


    public static IMap Empty = new EmptyMap();

}