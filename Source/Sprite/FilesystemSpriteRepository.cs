using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using NLog;

namespace y1000.Source.Sprite;

public class FilesystemSpriteRepository: AbstractSpriteRepository
{
    public static readonly FilesystemSpriteRepository Instance = new();

    private static readonly ILogger LOG = LogManager.GetCurrentClassLogger();
    private static readonly string DIR_PATH = "D:\\work\\sprite\\";
    private FilesystemSpriteRepository()
    {
    }

    public AtzSprite Load(string name)
    {
        var lines = File.ReadLines("D:\\work\\sprite\\a01\\offset.txt");
        foreach (var line in lines)
        {
            LOG.Info("Name {0}.", line );
        }
        return new AtzSprite(new Texture2D[1], new Vector2[1]);
    }

    private Vector2[] ParseVectors(IEnumerable<string> lines)
    {
        return (from line in lines where line.Contains(',') select ParseLine(line)).ToArray();
    }

    public override AtzSprite LoadByName(string name, Vector2? offset = null)
    {
        var spriteDirPath = DIR_PATH + name.ToLower() + "\\";
        var offsets = File.ReadLines(spriteDirPath + "offset.txt");
        var vectors = ParseVectors(offsets);
        var strings = Directory.GetFiles(spriteDirPath);
        List<Texture2D> texture2Ds = new List<Texture2D>();
        foreach (var filename in strings)
        {
            if (filename.EndsWith("png"))
            {
                var image = Image.LoadFromFile(filename);
                texture2Ds.Add(ImageTexture.CreateFromImage(image));
            }
        }
        if (offset.HasValue)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i] += offset.Value;
            }
        }
        if (texture2Ds.Count != vectors.Length)
        {
            LOG.Error("Invalid dir {0}.", name);
        }
        return new AtzSprite(texture2Ds.ToArray(), vectors);
    }

    public override AtzSprite LoadByPath(string path, Vector2? offset = null)
    {
        return new AtzSprite(new Texture2D[1], new Vector2[1]);
    }
}