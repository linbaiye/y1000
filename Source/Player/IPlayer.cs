using Godot;
using y1000.code;
using y1000.code.creatures;
using y1000.code.entity;
using y1000.code.player;

namespace y1000.Source.Player;

public interface IPlayer : IEntity
{
    bool IsMale { get; }
    
    Direction Direction { get; }
    
    OffsetTexture BodyOffsetTexture { get; }
    
    Vector2I Coordinate { get; }
}