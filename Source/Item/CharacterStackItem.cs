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

    public static readonly string MoneyName = "钱币";

    public override string ToString()
    {
        return ItemName + ":" + Number;
    }
}