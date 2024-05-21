namespace y1000.Source.Item;

public interface ICharacterItem : IItem
{
    long Id { get; }
    
    int IconId { get; }
    
    public string Name { get; }
    
}