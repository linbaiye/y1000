﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using NLog;

namespace y1000.Source.Sprite;

public class FilesystemSpriteRepository: AbstractSpriteRepository, ISpriteRepository
{
    public static readonly FilesystemSpriteRepository Instance = new();

    private static readonly ILogger LOG = LogManager.GetCurrentClassLogger();
    private static readonly string DIR_PATH = "../sprite/";
    //private static readonly string DIR_PATH = "D:/work/sprite/";
    private const bool CacheEnabled = true;
    private static readonly IDictionary<string, AtzSprite> Cache = new Dictionary<string, AtzSprite>();
    private Vector2[] ParseVectors(IEnumerable<string> lines)
    {
        return (from line in lines where line.Contains(',') select ParseLine(line)).ToArray();
    }

    public override AtzSprite LoadByNumberAndOffset(string name, Vector2? offset = null)
    {
        if (Cache.TryGetValue(name, out var sprite))
        {
            return sprite;
        }
        var spriteDirPath = DIR_PATH + name.ToLower() + "/";
        var offsets = File.ReadLines(spriteDirPath + "offset.txt");
        var vectors = ParseVectors(offsets);
        var sizefile = File.ReadLines(spriteDirPath + "size.txt");
        var sizes = ParseVectors(sizefile);
        List<Texture2D> texture2Ds = new List<Texture2D>();
        for (int i = 0; i < vectors.Length ; i++)
        {
            var filename = spriteDirPath + "000" + i.ToString("D3") + ".png";
            if (filename.EndsWith("png"))
            {
                var image = Image.LoadFromFile(filename);
                texture2Ds.Add(ImageTexture.CreateFromImage(image));
            }
            if (offset.HasValue)
            {
                vectors[i] += offset.Value;
            }
        }
        if (texture2Ds.Count != vectors.Length)
        {
            LOG.Error("Invalid dir {0}.", name);
        }
        LOG.Debug("Loaded {0}, {1} pictures in total.", name, texture2Ds.Count);
        AtzSprite atzSprite = new AtzSprite(texture2Ds.ToArray(), vectors, sizes);
        if (CacheEnabled)
        {
            Cache.TryAdd(name,atzSprite);
        }
        return atzSprite;
    }

    public bool Exists(string number)
    {
        return Directory.Exists(DIR_PATH + number.ToLower());
    }
}
