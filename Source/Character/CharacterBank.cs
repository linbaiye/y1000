using System;
using System.Collections.Generic;
using y1000.Source.Item;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character;

public class CharacterBank
{
    private readonly Dictionary<int, IItem> _items;
    public CharacterBank(int unlocked, List<IItem> items)
    {
        _items = new Dictionary<int, IItem>();
        for (int i = 0; i < items.Count; i++)
        {
            _items[i + 1] = items[i];
        }
        Unlocked = unlocked;
    }

    public int Capacity => 40;
    
    public int Unlocked { get; }

    public void ForeachSlot(Action<int, IItem> action)
    {
        for (int i = 1; i <= Capacity ; i++)
        {
            if (_items.TryGetValue(i, out var item))
            {
                action.Invoke(i, item);
            }
        }
    }

    public static CharacterBank Create(ItemFactory itemFactory, OpenBankMessage message)
    {
        List<IItem> items = new List<IItem>();
        foreach (var inventoryItemMessage in message.ItemMessages)
        {
            var characterItem = itemFactory.CreateCharacterItem(inventoryItemMessage);
            items.Add(characterItem);
        }
        return new CharacterBank(message.Unlocked, items);
    }

}