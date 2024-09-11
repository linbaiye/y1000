using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Godot;
using NLog;
using FileAccess = System.IO.FileAccess;

namespace y1000.Source.Map;

public class ZipFileMapObjectRepository : IMapObjectRepository
{
    private const string DirPath = "../map/";
    
    private const string ObjectDirName = "object";
    private const string TileDirName =  "tile";
    private const string RoofDirName = "roof";
    
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    private byte[] _buffer = new byte[1024 * 1024 * 10];
    
    private ImageTexture? ImageTextureFromEntry(ZipArchiveEntry entry)
    {
        if (!entry.FullName.EndsWith(".png"))
        {
            return null;
        }
        // DirAccess.RemoveAbsolute("/Users/ab000785/learn/tmp.png");
        // entry.ExtractToFile("/Users/ab000785/learn/tmp.png");
        // var loadFromFile = Image.LoadFromFile("/Users/ab000785/learn/tmp.png");
        var stream = entry.Open();
        byte[] bytes;
        using (var ms = new MemoryStream())
        {
            stream.CopyTo(ms);
            bytes = ms.ToArray();
            var image = new Image();
            var error = image.LoadPngFromBuffer(bytes);
            if (error == Error.Ok)
            {
                LOGGER.Debug("{0} Ok.", entry.FullName);
            }
        }
        // var read = stream.Read(_buffer, 0,(int)entry.Length);
        // LOGGER.Debug("Read {0} bytes.", read);
        // var image = new Image();
        // var error = image.LoadPngFromBuffer(_buffer);
        // if (error != Error.Ok)
        // {
        //     LOGGER.Error("Png error {0}.", error);
        // }
        // var jpgError = image.LoadJpgFromBuffer(_buffer);
        // if (jpgError != Error.Ok)
        //     LOGGER.Error("Jpg error {0}.", jpgError);
        // var bmp = image.LoadBmpFromBuffer(_buffer);
        // if (bmp != Error.Ok)
        //     LOGGER.Error("bmp error {0}.", bmp);


        return null;
    }
    
    

    public IDictionary<int, Texture2D> LoadTiles(string tileName)
    {
        var texture2Ds = new Dictionary<int, Texture2D>();
        using var zipArchive = ZipFile.Open(DirPath + tileName + ".zip", ZipArchiveMode.Read);
        var tilepath = tileName + "/" + TileDirName + "/";
        foreach (var entry in zipArchive.Entries)
        {
            if (!entry.FullName.StartsWith(tilepath))
                continue;
            LOGGER.Debug("{0}", entry.FullName);
            var imageTextureFromEntry = ImageTextureFromEntry(entry);
            if (imageTextureFromEntry != null)
            {
                var name = Path.GetFileNameWithoutExtension(entry.FullName);
                texture2Ds.Add(name.ToInt(), imageTextureFromEntry);
            }
        }
        return texture2Ds;
    }

    private IDictionary<int, MapObject> LoadObjects(string mapName, string objName)
    {
        using var zipArchive = ZipFile.Open(DirPath + mapName + ".zip", ZipArchiveMode.Read);
        IDictionary<int, MapObject> dictionary = new Dictionary<int, MapObject>();
        var zipArchiveEntry = zipArchive.GetEntry(mapName + "/" + objName + "/");
        if (zipArchiveEntry == null)
        {
            return dictionary;
        }

        var stream = zipArchiveEntry.Open();
        using var dir = new ZipArchive(stream);
        foreach (var entry in dir.Entries)
        {
            if (!entry.FullName.EndsWith("/"))
            {
                continue;
            }
            LOGGER.Debug("Extracting dir {0}.", entry.FullName);
            using var objDir = new ZipArchive(entry.Open());
            var jsonFile = objDir.GetEntry("struct.json");
            if (jsonFile != null)
            {
                LOGGER.Debug("Found json.");
            }
            foreach (var objDirEntry in objDir.Entries)
            {
                LOGGER.Debug(objDirEntry.FullName);
            }

            /*var bytes = File.ReadAllText(objDir + "/struct.json");
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
            mapObjects.TryAdd(name.ToInt(), new MapObject(texture2Ds, offset, name.ToInt()));*/
        }

        return dictionary;
    }

    public IDictionary<int, MapObject> LoadObjects(string name)
    {
        return LoadObjects(name, ObjectDirName);
    }

    public bool HasRoof(string mapName)
    {
        using var zipArchive = ZipFile.Open(DirPath + mapName + ".zip", ZipArchiveMode.Read);
        return zipArchive.GetEntry(mapName + "/" + RoofDirName) != null;
    }

    public IDictionary<int, MapObject> LoadRoof(string mapName)
    {
        if (!HasRoof(mapName))
        {
            return new Dictionary<int, MapObject>();
        }
        return LoadObjects(mapName, RoofDirName);
    }
}