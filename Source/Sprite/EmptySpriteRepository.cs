using Godot;

namespace y1000.Source.Sprite;

public class EmptySpriteRepository : ISpriteRepository
{
    public static readonly EmptySpriteRepository Instance = new EmptySpriteRepository();
    private EmptySpriteRepository() {}
    
    public AtzSprite LoadByNumberAndOffset(string name, Vector2? offset = null)
    {
        throw new System.NotImplementedException();
    }

    public AtzSprite LoadByPath(string path, Vector2? offset = null)
    {
        throw new System.NotImplementedException();
    }

    public AtzSprite LoadByNpcName(string name)
    {
        throw new System.NotImplementedException();
    }
}