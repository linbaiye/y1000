using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ToggleKungFuEvent : IClientEvent
{
    private readonly int _page;
    private readonly int _slot;

    public ToggleKungFuEvent(int page, int slot)
    {
        _page = page;
        _slot = slot;
    }

    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            ToggleKungFu = new ClientToggleKungFuPacket()
            {
                Tab = _page,
                Slot = _slot
            }
        };
    }
}