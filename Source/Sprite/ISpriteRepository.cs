using Godot;

namespace y1000.Source.Sprite;

public interface ISpriteRepository
{
    private static readonly Vector2 DEFAULT_VECTOR = new (16, -12);
    
    public static readonly ISpriteRepository Instance = ZipFileSpriteRepository.Instance;
    
    AtzSprite LoadByNumberAndOffset(string name, Vector2? offset = null);

    AtzSprite LoadByNumber(string number) => LoadByNumberAndOffset(number, DEFAULT_VECTOR);

    bool Exists(string number)
    {
        return false;
    }
}