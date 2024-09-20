using Godot;
using y1000.Source.Entity;

namespace y1000.Source.Creature;

public interface ICreature : IEntity, IBody
{
    Direction Direction { get; }
    
}