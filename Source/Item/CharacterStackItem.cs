namespace y1000.Source.Item;

public class CharacterStackItem : ICharacterItem
{
    public CharacterStackItem(int iconId, string name, int number)
    {
        IconId = iconId;
        Name = name;
        Number = number;
    }

    public int IconId { get; }
    
    public string Name { get; }
    
    public int Number { get; }

    public ItemType Type => ItemType.STACK;
}