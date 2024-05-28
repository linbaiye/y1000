using Godot;

namespace y1000.Source.Sprite;

public interface ISpriteRepository
{
    private static readonly Vector2 DEFAULT_VECTOR = new (16, -12);
    AtzSprite LoadByName(string name, Vector2? offset = null);
    
    AtzSprite LoadByPath(string path, Vector2? offset = null);

    AtzSprite LoadByNameAndOffset(string name) => LoadByName(name, DEFAULT_VECTOR);
}