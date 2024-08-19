using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public sealed class ClientSwapKungFuSlotEvent : IClientEvent
{
    public ClientSwapKungFuSlotEvent(int page, int slot1, int slot2)
    {
        Page = page;
        Slot1 = slot1;
        Slot2 = slot2;
    }

    private int Page { get; }
    private int Slot1 { get; }
    private int Slot2 { get; }
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            SwapKungFuSlot = new ClientSwapKungFuSlotPacket()
            {
                Page = Page,
                Slot1 = Slot1,
                Slot2 = Slot2,
            }
        };
    }
}