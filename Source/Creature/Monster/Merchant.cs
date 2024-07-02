using System.Collections.Generic;
using System.Linq;
using y1000.Source.Map;
using y1000.Source.Networking;

namespace y1000.Source.Creature.Monster;

public partial class Merchant : Monster
{
    public List<Item> SellItems { get; set; } = new();

    public List<Item> BuyItems { get; set; } = new ();

    public int AvatarSpriteNumber { get; private set; } = 0;
    
    public class Item
    {
        public Item(string name, int price, int iconId)
        {
            Price = price;
            IconId = iconId;
            Name = name;
        }
        public string Name { get; }
        public int Price { get; }
        public int IconId { get; }
    }

    public Item? FindInSell(string name)
    {
        return SellItems.FirstOrDefault(i => i.Name.Equals(name));
    }

    public bool Buys(string name)
    {
        return BuyItems.Any(i => i.Name.Equals(name));
    }
    
    public void InitializeMerchant(
        NpcInterpolation npcInterpolation, IMap map,
        List<Item> sell, List<Item> buy, int avNumber)
    {
        Initialize(this, npcInterpolation, map);
        SellItems = sell;
        BuyItems = buy;
        AvatarSpriteNumber = avNumber;
    }
}