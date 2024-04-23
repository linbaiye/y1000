using Godot;
using y1000.code.player;

namespace y1000.Source.Creature
{
    public interface IBody
    {
        OffsetTexture BodyOffsetTexture { get; }
        
        Vector2 Position { get; }
    }
}