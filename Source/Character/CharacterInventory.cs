using System;
using System.Collections.Generic;
using System.Linq;
using y1000.Source.Item;

namespace y1000.Source.Character;

public class CharacterInventory
{
    private const int MaxSize = 30;
    
    private readonly IDictionary<int, ICharacterItem> _items = new Dictionary<int, ICharacterItem>(MaxSize);

    public bool IsFull => _items.Count >= MaxSize;

    public void AddItem(ICharacterItem item)
    {
        if (IsFull) {
            throw new IndexOutOfRangeException();
        }
        for (var i = 0; i < MaxSize; i++)
        {
            if (_items.TryAdd(i, item))
            {
                break;
            }
        }
    }

    public void AddItems(IEnumerable<ICharacterItem> items)
    {
        if (items.Count() + _items.Count >= MaxSize)
        {
            throw new IndexOutOfRangeException();
        }
        foreach (var item in items)
        {
            AddItem(item);
        }
    }
}