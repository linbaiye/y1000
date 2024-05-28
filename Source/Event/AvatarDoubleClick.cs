using System;
using y1000.Source.Item;

namespace y1000.Source.Event;

public class AvatarDoubleClick : EventArgs
{
    public AvatarDoubleClick(EquipmentType equipmentType)
    {
        EquipmentType = equipmentType;
    }

    public EquipmentType EquipmentType { get; }
    
}