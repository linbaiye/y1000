namespace y1000.Source.Item;

public class AccumulativeItem : ICharacterItem
{
    public AccumulativeItem(long id, int iconId, string name, int number, ItemType type)
    {
        Id = id;
        IconId = iconId;
        Name = name;
        Number = number;
        Type = type;
    }

    public long Id { get; }
    
    public int IconId { get; }
    
    public string Name { get; }
    
    public int Number { get; }
    
    public ItemType Type { get; }
}