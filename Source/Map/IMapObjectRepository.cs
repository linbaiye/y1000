using System.Collections.Generic;
using Godot;

namespace y1000.Source.Map;

public interface IMapObjectRepository
{
    
    public static readonly IMapObjectRepository Instance = ZipFileMapObjectRepository.Instance;
    
    IDictionary<int, Texture2D> LoadTiles(string tileName);

    IDictionary<int, MapObject> LoadObjects(string name);

    bool HasRoof(string mapName);
    
    IDictionary<int, MapObject> LoadRoof(string mapName);
}