using Godot;
using y1000.Source.Creature;
using y1000.Source.Entity;

namespace y1000.Source.Map;

public class EmptyMap: IMap
{
    public bool Movable(Vector2I coordinate)
    {
        throw new System.NotImplementedException();
    }

    public void Occupy(IEntity entity)
    {
        throw new System.NotImplementedException();
    }

    public void Free(IEntity creature)
    {
        throw new System.NotImplementedException();
    }

}