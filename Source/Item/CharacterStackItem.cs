namespace y1000.Source.Item;

public class CharacterStackItem : IItem
{
    public CharacterStackItem(int iconId, string name, long number, int color = 0)
    {
        IconId = iconId;
        ItemName = name;
        Number = number;
        Color = color;
    }

    public int IconId { get; }
    public int Color { get; }
    public long Number { get; set; }
    public string ItemName { get; }

    public CharacterStackItem Duplicate()
    {
        return new CharacterStackItem(IconId, ItemName, Number, Color);
    }
    
    public override string ToString()
    {
        return ItemName + ":" + Number;
    }
}