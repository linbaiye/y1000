namespace y1000.Source.Item;

public abstract class AbstractArmor : IEquipment
{
    protected AbstractArmor(string firstAtzName, string secondAtzName, string thirdAtzName, string fourthAtzName, string fifthAtzName, string name, int color = 0)
    {
        FirstAtzName = firstAtzName;
        SecondAtzName = secondAtzName;
        ThirdAtzName = thirdAtzName;
        FourthAtzName = fourthAtzName;
        Name = name;
        Color = color;
        FifthAtzName = fifthAtzName;
    }
    
    public int Color { get; }
    
    public string FirstAtzName { get; }
    
    public string SecondAtzName { get; }
    
    public string ThirdAtzName { get; }
    
    public string FourthAtzName { get; }
    
    public string FifthAtzName { get; }
    
    public string Name { get; }

    public abstract EquipmentType EquipmentType { get; }
}