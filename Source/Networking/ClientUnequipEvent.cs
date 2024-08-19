using Source.Networking.Protobuf;
using y1000.Source.Input;
using y1000.Source.Item;

namespace y1000.Source.Networking;

public class ClientUnequipEvent : IClientEvent
{
    public ClientUnequipEvent(EquipmentType type)
    {
        Type = type;
    }

    private EquipmentType Type { get; }
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            Unequip = new ClientUnequipPacket()
            {
                Type = (int)Type
            }
        };
    }
}