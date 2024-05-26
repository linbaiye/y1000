using Godot;

namespace y1000.Source.Sprite;

public interface ISpriteRepository
{
    AtzSprite LoadByName(string name, Vector2? offset = null);
    
    AtzSprite LoadByPath(string path, Vector2? offset = null);
}