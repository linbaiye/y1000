using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientRightClickItemEvent: IClientEvent
{
    public ClientRightClickItemEvent(RightClickType type, int slot, int page)
    {
        Type = type;
        Slot = slot;
        Page = page;
    }

    private RightClickType Type { get; }
    private int Slot { get; }
    private int Page { get; } = 0;
    
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            Right
        };
    }
}