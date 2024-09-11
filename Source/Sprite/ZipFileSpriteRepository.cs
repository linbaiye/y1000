using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Godot;
using NLog;
using y1000.Source.Util;

namespace y1000.Source.Sprite;

public class ZipFileSpriteRepository : AbstractSpriteRepository
{
    public static readonly ZipFileSpriteRepository Instance = new ();
    private ZipFileSpriteRepository() {}
    
    private const string DirPath = "../sprite/";
    
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    private Vector2[] ParseVectors(IEnumerable<string> lines)
    {
        return (from line in lines where line.Contains(',') select ParseLine(line)).ToArray();
    }

    public override AtzSprite LoadByNumberAndOffset(string name, Vector2? offset = null)
    {
        using var zipArchive = ZipFile.Open(DirPath + name.ToLower()+ ".zip", ZipArchiveMode.Read);
        var offsetEntry = zipArchive.GetEntry("offset.txt");
        if (offsetEntry == null)
        {
            throw new FileNotFoundException("Bad atz zip file: " + name);
        }
        var sizeEntry  = zipArchive.GetEntry("size.txt");
        if (sizeEntry == null)
        {
            throw new FileNotFoundException("Bad atz zip file: " + name);
        }
        var vectors = ParseVectors(offsetEntry.ReadAsString().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None));
        var sizes = ParseVectors(sizeEntry.ReadAsString().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None));
        List<Texture2D> texture2Ds = new List<Texture2D>();
        for (int i = 0; i < vectors.Length; i++)
        {
            var filename = "000" + i.ToString("D3") + ".png";
            var zipArchiveEntry = zipArchive.GetEntry(filename);
            var texture = zipArchiveEntry?.ReadAsTexture();
            if (texture == null)
            {
                continue;
            }
            texture2Ds.Add(texture);
            if (offset.HasValue)
            {
                vectors[i] += offset.Value;
            }
        }
        if (texture2Ds.Count != vectors.Length)
        {
            LOGGER.Error("Invalid atz {0}.", name);
        }
        LOGGER.Debug("Loaded {0}, {1} pictures in total.", name, texture2Ds.Count);
        return new AtzSprite(texture2Ds.ToArray(), vectors, sizes);
    }

}