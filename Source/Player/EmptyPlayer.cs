using Godot;
using y1000.code;
using y1000.code.player;
using y1000.Source.Animation;
using y1000.Source.Creature;

namespace y1000.Source.Player;

public class EmptyPlayer : IPlayer
{
    public string EntityName { get; }
    public long Id { get; }
    public void Free()
    {
        throw new System.NotImplementedException();
    }

    public bool IsMale { get; }
    public Direction Direction { get; }
    
    public OffsetTexture BodyOffsetTexture { get; }
    
    public Vector2I Coordinate { get; }
}