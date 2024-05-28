using System;
using y1000.Source.Item;

namespace y1000.Source.Event;

public class EquipmentChangedEvent : EventArgs, IUiEvent
{
    public EquipmentChangedEvent(EquipmentType equipmentType)
    {
        EquipmentType = equipmentType;
    }

    public EquipmentType EquipmentType { get; }
    
}