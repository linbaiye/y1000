namespace y1000.Source.Item;

public class CharacterItem : ICharacterItem
{
    public CharacterItem(int iconId, string itemName)
    {
        ItemName = itemName;
        IconId = iconId;
    }

    public string ItemName { get; }
    
    public int IconId { get; }
}