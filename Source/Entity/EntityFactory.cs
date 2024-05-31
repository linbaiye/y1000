using System;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.Networking;
using y1000.Source.Sprite;
using y1000.Source.Util;

namespace y1000.Source.Entity;

public class EntityFactory
{

    private readonly ItemSdbReader _itemDb;
    
    private readonly IconReader _iconReader;

    private readonly EventMediator _eventMediator;
    public EntityFactory(EventMediator eventMediator)
    {
        _itemDb = ItemSdbReader.ItemSdb;
        _iconReader = IconReader.ItemIconReader;
        _eventMediator = eventMediator;
    }

    public GroundedItem CreateGroundedItem(ShowItemMessage message)
    {
        var iconId = _itemDb.GetIconId(message.Name);
        var texture2D = _iconReader.Get(iconId);
        if (texture2D == null)
        {
            throw new NotImplementedException(message.Name + " does not have icon.");
        }
        return GroundedItem.Build(message, texture2D, _eventMediator);
    }
}