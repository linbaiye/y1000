using System;
using System.Collections.Generic;
using Godot;

namespace y1000.Source.Item;

public class ItemTextureReader
{
    private readonly Dictionary<string, Texture2D> _textures;

    private ItemTextureReader(Dictionary<string, Texture2D> textures)
    {
        _textures = textures;
    }

    public Texture2D? Get(string name)
    {
        return _textures.GetValueOrDefault(name);
    }
    
    public Texture2D? Get(int name)
    {
        return _textures.GetValueOrDefault("000" + name.ToString("000") + ".png");
    }

    public static ItemTextureReader LoadItems()
    {
        var dirpath = "res://sprite/item";
        var dirAccess = DirAccess.Open(dirpath);
        var files = dirAccess.GetFiles();
        if (files == null)
        {
            throw new NotSupportedException();
        }

        Dictionary<string, Texture2D> result = new();
        foreach (var file in files)
        {
            if (!file.EndsWith("png"))
            {
                continue;
            }
            if (ResourceLoader.Load(dirpath + "/" + file) is Texture2D texture)
            {
                result.TryAdd(file, texture);
            }
        }
        return new ItemTextureReader(result);
    }
}