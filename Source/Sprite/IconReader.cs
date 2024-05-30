using System;
using System.Collections.Generic;
using Godot;

namespace y1000.Source.Sprite;

public class IconReader
{
    private readonly Dictionary<string, Texture2D> _textures;

    public static readonly IconReader ItemIconReader = LoadItems();
    
    public static readonly IconReader KungFuIconReader = LoadKungFu();

    private readonly string _extname;

    private IconReader(Dictionary<string, Texture2D> textures, string extname)
    {
        _textures = textures;
        _extname = extname;
    }

    
    public Texture2D? Get(int iconId)
    {
        return _textures.GetValueOrDefault("000" + iconId.ToString("000") + "." + _extname);
    }
    
    private static IconReader LoadItems(string dirpath, string extName)
    {
        var dirAccess = DirAccess.Open(dirpath);
        var files = dirAccess.GetFiles();
        if (files == null)
        {
            throw new NotSupportedException();
        }

        Dictionary<string, Texture2D> result = new();
        foreach (var file in files)
        {
            if (!file.EndsWith(extName))
            {
                continue;
            }
            if (ResourceLoader.Load(dirpath + "/" + file) is Texture2D texture)
            {
                result.TryAdd(file, texture);
            }
        }
        return new IconReader(result, extName);
    }
    
    private static IconReader LoadKungFu()
    {
        return LoadItems("res://assets/newmagic", "bmp");
    }

    private static IconReader LoadItems()
    {
        return LoadItems("res://sprite/item", "png");
    }
    
}