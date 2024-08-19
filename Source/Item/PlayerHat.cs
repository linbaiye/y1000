namespace y1000.Source.Item;

public class PlayerHat : AbstractArmor
{
    public PlayerHat(string firstAtzName, string secondAtzName, string thirdAtzName, string fourthAtzName, string fifthAtzName, string name, int color) : base(firstAtzName, secondAtzName, thirdAtzName, fourthAtzName, fifthAtzName, name, color)
    {
    }

    public override EquipmentType EquipmentType => EquipmentType.HAT;
}