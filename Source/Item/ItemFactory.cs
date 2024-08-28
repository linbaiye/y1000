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


    private readonly ItemSdbReader _itemDb;

    private ItemFactory()
    {
        _itemDb = ItemSdbReader.ItemSdb;
    }

    public IItem CreateCharacterItem(InventoryItemMessage message)
    {
        bool canstack = _itemDb.CanStack(message.Name);
        if (canstack)
        {
            return new CharacterStackItem(_itemDb.GetIconId(message.Name), message.Name, message.Number, message.Color);
        }
        return new CharacterItem(_itemDb.GetIconId(message.Name), message.Name, message.Color);
    }

    public bool IsStackItem(string name)
    {
        return _itemDb.CanStack(name);
    }

    public IItem CreateCharacterItem(string name, int color = 0, long number = 0)
    {
        if (number == 0)
        {
            return new CharacterItem(_itemDb.GetIconId(name), name, color);
        }
        return new CharacterStackItem(_itemDb.GetIconId(name), name, number, color);
    }

    public IItem CreateCharacterItem(string name, long number = 0)
    {
        return CreateCharacterItem(name, 0, number);
    }
}