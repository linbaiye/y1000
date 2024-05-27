namespace y1000.Source.Item;

public class PlayerChest
{
    public PlayerChest(string first, string second, string third, string fourth, string name, string fifth)
    {
        First = first;
        Second = second;
        Third = third;
        Fourth = fourth;
        Name = name;
        Fifth = fifth;
    }
    
    public string First { get; }
    
    public string Second { get; }
    
    public string Third { get; }
    
    public string Fourth { get; }
    
    public string Fifth { get; }
    
    public string Name { get; }
    
}