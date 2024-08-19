namespace y1000.Source.Item;

public class Wrist : AbstractArmor
{
    public Wrist(
        string firstAtzName, string secondAtzName, string thirdAtzName, string fourthAtzName, string fifthAtzName,
        string firstAtzName1, string secondAtzName1, string thirdAtzName1, string fourthAtzName1, string fifthAtzName1,
        string name, int color
        ) : base(firstAtzName, secondAtzName, thirdAtzName, fourthAtzName, fifthAtzName, name, color)
    {
        FirstAtz1 = firstAtzName1;
        SecondAtz1 = secondAtzName1;
        ThirdAtz1 = thirdAtzName1;
        FourthAtz1 = fourthAtzName1;
        FifthAtz1 = fifthAtzName1;
    }

    public string FirstAtz1 { get; }
    public string SecondAtz1 { get; }
    public string ThirdAtz1 { get; }
    public string FourthAtz1 { get; }
    
    public string FifthAtz1 { get; }

    public override EquipmentType EquipmentType => EquipmentType.WRIST;
}