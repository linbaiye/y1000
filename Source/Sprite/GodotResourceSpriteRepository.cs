using System;
using System.Collections.Generic;
using System.IO;
using Godot;

namespace y1000.Source.Sprite;

public class GodotResourceSpriteRepository : AbstractSpriteRepository
{
    private Vector2[] ReadOffsets(string text)
    {
        var tokens = text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        List<Vector2> list = new List<Vector2>();
        foreach (var s in tokens){ 
            if (!s.Contains(',')) {
                continue;
            }
            list.Add(ParseLine(s));
        }
                
        return list.ToArray();
    }
    
    private AtzSprite LoadFromResource(string resDir, Vector2? offset = null) {
        if (!resDir.StartsWith("res://")){
            resDir = "res://" + resDir;
        }
        if (!resDir.EndsWith("/")) {
            resDir += "/";
        }
        if (!Godot.FileAccess.FileExists(resDir + "offset.txt")) {
            throw new FileNotFoundException();
        }
        Godot.FileAccess fileAccess = Godot.FileAccess.Open(resDir + "offset.txt", Godot.FileAccess.ModeFlags.Read);
        Vector2[] vectors = ReadOffsets(fileAccess.GetAsText());
        if (vectors.Length <= 0) {
            throw new FileNotFoundException("Empty offset file.");
        }
        Texture2D[] textures = new Texture2D[vectors.Length];
        for (int i = 0; i < vectors.Length ; i++) {
            var texture = ResourceLoader.Load(resDir + "000" + i.ToString("D3") + ".png") as Texture2D;
            textures[i] = texture;
            if (offset.HasValue)
            {
                vectors[i] += offset.Value;
            }
        }
        fileAccess.Dispose();
        return new AtzSprite(textures, vectors);
    }

    public override AtzSprite LoadByNameAndOffset(string name, Vector2? offset = null)
    {
        return LoadFromResource("res://sprite/" + name + "/", offset);
    }

    public override AtzSprite LoadByPath(string path, Vector2? offset = null)
    {
        return LoadFromResource(path, offset);
    }
}