namespace y1000.Source.Item;

public class PlayerChest : AbstractArmor
{
    public PlayerChest(string firstAtzName, string secondAtzName, string thirdAtzName, string fourthAtzName, string fifthAtzName, string name) : base(firstAtzName, secondAtzName, thirdAtzName, fourthAtzName, fifthAtzName, name)
    {
    }

    public override EquipmentType EquipmentType => EquipmentType.CHEST;
}