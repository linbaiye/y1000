namespace y1000.Source.Item;

public abstract class AbstractArmor
{
    protected AbstractArmor(string firstAtzName, string secondAtzName, string thirdAtzName, string fourthAtzName, string fifthAtzName, string name)
    {
        FirstAtzName = firstAtzName;
        SecondAtzName = secondAtzName;
        ThirdAtzName = thirdAtzName;
        FourthAtzName = fourthAtzName;
        Name = name;
        FifthAtzName = fifthAtzName;
    }
    
    public string FirstAtzName { get; }
    
    public string SecondAtzName { get; }
    
    public string ThirdAtzName { get; }
    
    public string FourthAtzName { get; }
    
    public string FifthAtzName { get; }
    
    public string Name { get; }
    
}