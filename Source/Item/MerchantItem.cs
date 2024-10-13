

namespace y1000.Source.Item
{
    public class MerchantItem : IItem
    {
        public MerchantItem(string itemName, int iconId, int color, int price, bool canStack = false)
        {
            ItemName = itemName;
            Color = color;
            IconId = iconId;
            Price = price;
            CanStack = canStack;
        }

        public string ItemName { get; }

        public int IconId { get; }

        public int Color { get; }

        public int Price { get; }

        public bool CanStack {get;}

        public override string ToString()
        {
            return ItemName;
        }

        public IItem ToItem(long number)
        {
            return CanStack ? new CharacterStackItem(IconId, ItemName, number, Color) :
                new CharacterItem(IconId, ItemName, Color);
        }
    }
}