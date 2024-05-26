namespace y1000.Source.Item;

public class CharacterStackItem : ICharacterItem
{
    public CharacterStackItem(int iconId, string name, int number)
    {
        IconId = iconId;
        ItemName = name;
        Number = number;
    }

    public int IconId { get; }
    
    public int Number { get; set; }

    public string ItemName { get; }
}