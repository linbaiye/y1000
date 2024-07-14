using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientRightClickEvent: IClientEvent
{
    public ClientRightClickEvent(RightClickType type, int slot = 0 , int page = 0)
    {
        Type = type;
        Slot = slot;
        Page = page;
    }
    

    private RightClickType Type { get; }
    private int Slot { get; } = 0;
    private int Page { get; } = 0;
    
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            RightClick = new RightClickPacket()
            {
                Page = Page,
                SlotId = Slot,
                Type = (int)Type
            }
        };
    }
}