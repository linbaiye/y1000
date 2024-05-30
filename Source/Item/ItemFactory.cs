using System;
using System.Collections.Generic;
using y1000.Source.Animation;
using y1000.Source.Entity;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Sprite;
using y1000.Source.Util;

namespace y1000.Source.Item;

public class ItemFactory
{

    public static readonly ItemFactory Instance = new ();

    private readonly IconReader _textureReader;

    private readonly ItemSdbReader _itemDb;

    private ItemFactory()
    {
        _textureReader = IconReader.ItemIconReader;
        _itemDb = ItemSdbReader.ItemSdb;
    }

    public ICharacterItem CreateCharacterItem(JoinedRealmMessage.InventoryItemMessage message)
    {
        bool canstack = _itemDb.CanStack(message.Name);
        if (canstack)
        {
            return new CharacterStackItem(_itemDb.GetIconId(message.Name), message.Name, message.Number);
        }
        return new CharacterItem(_itemDb.GetIconId(message.Name), message.Name);
    }

    public ICharacterItem CreateCharacterItem(string name, int number = 0)
    {
        if (number == 0)
        {
            return new CharacterItem(_itemDb.GetIconId(name), name);
        }
        return new CharacterStackItem(_itemDb.GetIconId(name), name, number);
    }
}