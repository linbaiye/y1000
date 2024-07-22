using Godot;
using y1000.Source.Creature;
using y1000.Source.Entity;

namespace y1000.Source.Map;

public interface IMap
{
    bool Movable(Vector2I coordinate);

    void Occupy(IEntity entity);

    void Free(IEntity creature);


    public static IMap Empty = new EmptyMap();

}