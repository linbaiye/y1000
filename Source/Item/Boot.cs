﻿namespace y1000.Source.Item;

public class Boot : AbstractArmor
{
    public Boot(string firstAtzName, string secondAtzName, string thirdAtzName, string fourthAtzName, string fifthAtzName, string name, int color = 0) :
        base(firstAtzName, secondAtzName, thirdAtzName, fourthAtzName, fifthAtzName, name, color)
    {
    }

    public override EquipmentType EquipmentType => EquipmentType.BOOT;
}