using Godot;

namespace y1000.Source.Map;

public class MapObject
{
    public MapObject(Texture2D[] texture, Vector2 offset,int id)
    {
        Textures = texture;
        Offset = offset;
        Id = id;
    }

    public Texture2D[] Textures { get; }
		
    public Vector2 Offset { get;  }

    private int Id { get; }

    public string Name(int x, int y)
    {
        return "obj_" + Id + "_" + x + "_" + y;
    }
}