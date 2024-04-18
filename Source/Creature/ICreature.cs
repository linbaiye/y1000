using Godot;
using y1000.code;
using y1000.code.entity;

namespace y1000.Source.Creature;

public interface ICreature : IEntity
{
    Direction Direction { get; }
    
    Vector2I Coordinate { get; }
}