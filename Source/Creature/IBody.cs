using System.Numerics;
using y1000.Source.Animation;
using Vector2 = Godot.Vector2;

namespace y1000.Source.Creature
{
    public interface IBody
    {
        OffsetTexture BodyOffsetTexture { get; }
        
        Vector2 OffsetBodyPosition { get; }
        
        Vector2 BodyPosition { get; }
    }
}