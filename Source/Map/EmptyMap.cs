using Godot;
using y1000.Source.Creature;

namespace y1000.Source.Map;

public class EmptyMap: IMap
{
    public bool Movable(Vector2I coordinate)
    {
        throw new System.NotImplementedException();
    }

    public void Occupy(ICreature creature)
    {
        throw new System.NotImplementedException();
    }

    public void Free(ICreature creature)
    {
        throw new System.NotImplementedException();
    }
}