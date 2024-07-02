using System.Collections.Generic;
using System.Linq;

namespace y1000.Source.Item;

public class MerchantTrade
{

    private readonly List<Item> _items = new();
    
    public class Item
    {
        public Item(string name, long number, int slot)
        {
            Name = name;
            Number = number;
            Slot = slot;
        }

        public string Name { get; }
        
        public long Number { get; }
        
        public int Slot { get; }
    }

    public void Clear()
    {
        _items.Clear();
    }

    public bool IsEmpty => _items.Count == 0;

    public List<Item> Items => _items;

    public bool Contains(string name)
    {
        return _items.Any(i => i.Name.Equals(name));
    }
    
    public void Add(string name, long number, int slot) {
        _items.Add(new Item(name, number, slot));
    }
}