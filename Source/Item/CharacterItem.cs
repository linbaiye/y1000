namespace y1000.Source.Item;

public class CharacterItem : IItem
{
    public CharacterItem(int iconId, string itemName, int color)
    {
        ItemName = itemName;
        Color = color;
        IconId = iconId;
    }

    public string ItemName { get; }

    public int IconId { get; }
    public int Color { get; }

    public override string ToString()
    {
        return ItemName;
    }
}
