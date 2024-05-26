using Godot;

namespace y1000.Source.Sprite;

public abstract class AbstractSpriteRepository : ISpriteRepository
{
    protected Vector2 ParseLine(string s)
    {
        if (!s.Contains(','))
        {
            return new Vector2(0, 0);
        }
        var nobrackets = s.Replace("[", "").Replace("]", "");
        var numbers = nobrackets.Split(",");
        return numbers.Length == 2 ? 
            new Vector2(int.Parse(numbers[0].Trim()), int.Parse(numbers[1].Trim())) :
            new Vector2(0, 0);
    }
    
    public abstract AtzSprite LoadByName(string name, Vector2? offset = null);
    public abstract AtzSprite LoadByPath(string path, Vector2? offset = null);
}