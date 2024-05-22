namespace y1000.Source.Item;

public class AccumulativeItem : ICharacterItem
{
    public AccumulativeItem(int iconId, string name, int number, ItemType type)
    {
        IconId = iconId;
        Name = name;
        Number = number;
        Type = type;
    }

    public int IconId { get; }
    
    public string Name { get; }
    
    public int Number { get; }
    
    public ItemType Type { get; }
}