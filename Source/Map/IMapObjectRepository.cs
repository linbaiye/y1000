using System.Collections.Generic;
using Godot;

namespace y1000.Source.Map;

public interface IMapObjectRepository
{
    IDictionary<int, Texture2D> LoadTiles(string tileName);

    IDictionary<int, MapObject> LoadObjects(string name);
}