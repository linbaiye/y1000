using System;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.Networking;

namespace y1000.Source.Entity;

public class EntityFactory
{

    private readonly ItemDb _itemDb;
    
    private readonly ItemTextureReader _itemTextureReader;

    private readonly EventMediator _eventMediator;
    public EntityFactory(EventMediator eventMediator)
    {
        _itemDb = ItemDb.Instance;
        _itemTextureReader = ItemTextureReader.Instance;
        _eventMediator = eventMediator;
    }

    public GroundedItem CreateGroundedItem(ShowItemMessage message)
    {
        var iconId = _itemDb.GetIconId(message.Name);
        var texture2D = _itemTextureReader.Get(iconId);
        if (texture2D == null)
        {
            throw new NotImplementedException(message.Name + " does not have icon.");
        }
        return GroundedItem.Build(message, texture2D, _eventMediator);
    }
}