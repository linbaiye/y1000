using Godot;
using y1000.Source.Util;

namespace y1000.Source.Sprite;

public class IconRepository
{

    private readonly ItemSdbReader _itemSdbReader;
    
    public static readonly IconRepository Instance = new();

    private readonly IconReader _iconReader;
    private IconRepository()
    {
        _itemSdbReader = ItemSdbReader.ItemSdb;
        _iconReader = IconReader.ItemIconReader;
    }


    public Texture2D? LoadByName(string name)
    {
        if (!_itemSdbReader.Contains(name))
        {
            return null;
        }
        var iconId = _itemSdbReader.GetIconId(name);
        return _iconReader.Get(iconId);
    }


}