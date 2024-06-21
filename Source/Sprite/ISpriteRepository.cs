using Godot;

namespace y1000.Source.Sprite;

public interface ISpriteRepository
{
    private static readonly Vector2 DEFAULT_VECTOR = new (16, -12);
    AtzSprite LoadByNameAndOffset(string name, Vector2? offset = null);
    
    AtzSprite LoadByPath(string path, Vector2? offset = null);

    AtzSprite LoadByName(string name) => LoadByNameAndOffset(name, DEFAULT_VECTOR);

}