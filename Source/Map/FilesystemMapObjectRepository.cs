using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;
using NLog;

namespace y1000.Source.Map;

public class FilesystemMapObjectRepository : IMapObjectRepository
{
    private const string DirPath = "../map/";
    private const string ObjectDirName = "object";
    private const string TileDirName =  "tile";
    private const string RoofDirName = "roof";
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

    public static readonly FilesystemMapObjectRepository Instance = new();

    private FilesystemMapObjectRepository()
    {
    }

    public IDictionary<int, Texture2D> LoadTiles(string tileName)
    {
        var tileDirName = DirPath + tileName + "/" + TileDirName;
        var files = Directory.GetFiles(tileDirName);
        IDictionary<int, Texture2D> result = new Dictionary<int, Texture2D>();
        foreach(var path in files)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            if (!File.Exists(path))
            {
                continue;
            }

            var texture = CreateTextureFromPath(path);
            result.TryAdd(name.ToInt(), texture);
        }
        return result;
    }


    private ImageTexture CreateTextureFromPath(string path)
    {
        Image image = new Image();
        image.Load(path);
        var ret = ImageTexture.CreateFromImage(image);
        image.Dispose();
        return ret;
    }
    
    
    private struct Object2Json
    {
        public Object2Json()
        {
            Width = 0;
            Height = 0;
            X = 0;
            Y = 0;
            Number = 1;
            Delay = 0;
        }

        public byte Version => 2;

        public int Width { get; set; }

        public int Height { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public int Number { get; set; }
		
        public int Delay { get; set; }

        public static Object2Json FromJsonString(string jsonString)
        {
            return JsonSerializer.Deserialize<Object2Json>(jsonString);
        }
    }

    public IDictionary<int, MapObject> LoadObjects(string mapName)
    {
        return LoadObjectsByPath(DirPath + mapName + "/" + ObjectDirName);
    }

    private IDictionary<int, MapObject> LoadObjectsByPath(string dirpath)
    {
        var objectDirs = Directory.GetDirectories(dirpath);
        IDictionary<int, MapObject> mapObjects = new Dictionary<int, MapObject>();
        foreach (var objDir in objectDirs)
        {
            var bytes = File.ReadAllText(objDir + "/struct.json");
            var name = Path.GetFileNameWithoutExtension(objDir);
            Object2Json json = Object2Json.FromJsonString(bytes);
            var offset = new Vector2(json.X, json.Y);
            Texture2D[] texture2Ds = new Texture2D[json.Number];
            for (int i = 0; i < json.Number; i++)
            {
                var imagePath = objDir + "/" + i + ".png";
                var textureFromPath = CreateTextureFromPath(imagePath);
                texture2Ds[i] = textureFromPath;
            }
            mapObjects.TryAdd(name.ToInt(), new MapObject(texture2Ds, offset, name.ToInt()));
        }
        return mapObjects;
    }

    public IDictionary<int, MapObject> LoadRoof(string mapName)
    {
        return LoadObjectsByPath(DirPath + mapName + "/" + RoofDirName);
    }
}