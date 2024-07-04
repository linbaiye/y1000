namespace y1000.Source.Item;

public class CharacterStackItem : ICharacterItem
{
    public CharacterStackItem(int iconId, string name, long number)
    {
        IconId = iconId;
        ItemName = name;
        Number = number;
    }

    public int IconId { get; }
    
    public long Number { get; set; }

    public string ItemName { get; }

    public static readonly string MoneyName = "钱币";
    
}