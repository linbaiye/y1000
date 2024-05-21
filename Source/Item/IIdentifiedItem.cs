namespace y1000.Source.Item;

public interface IIdentifiedItem : IItem
{
    string ItemName { get; }
    
    int IconId { get; }
    
}