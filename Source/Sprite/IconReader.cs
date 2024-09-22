using System;
using System.Collections.Generic;
using Godot;
using NLog;

namespace y1000.Source.Sprite;

public class IconReader
{
    private readonly Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();

    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

    public static readonly IconReader ItemIconReader = LoadItems();
    
    public static readonly IconReader KungFuIconReader = LoadKungFu();

    private readonly string _extname;

    private readonly string _dirname;


    private IconReader(string dir, string extname)
    {
        _extname = extname;
        _dirname = dir;
    }

    
    public Texture2D? Get(int iconId)
    {
        var path = _dirname + "/" + "000" + iconId.ToString("000") + "." + _extname;
        if (ResourceLoader.Load(path) is Texture2D texture)
            return texture;
        return null;
    }
    
    private static IconReader LoadItems(string dirpath, string extName)
    {
        using var dirAccess = DirAccess.Open(dirpath);
        var files = dirAccess.GetFiles();
        if (files == null)
        {
            LOGGER.Debug("Nothing to load for {0}.", dirpath);
            throw new NotSupportedException();
        }

        Dictionary<string, Texture2D> result = new();
        foreach (var file in files)
        {
            if (!file.EndsWith(extName))
            {
                continue;
            }
            var path = dirpath + "/" + file;
            LOGGER.Debug("Loading img: {0}", path);
            if (ResourceLoader.Load(path) is Texture2D texture)
            {
                result.TryAdd(file, texture);
            }
        }
        return new IconReader(dirpath, extName);
    }
    
    private static IconReader LoadKungFu()
    {
        return new IconReader("res://assets/newmagic", "bmp");
    }

    private static IconReader LoadItems()
    {
        return new IconReader("res://sprite/item", "png");
    }
    
}