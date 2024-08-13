using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientDragPlayerEvent : IClientEvent
{
    public ClientDragPlayerEvent(long targetId, int ropeSlotId)
    {
        RopeSlotId = ropeSlotId;
        TargetId = targetId;
    }

    private long TargetId { get; }
    private int RopeSlotId { get; }
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            DragPlayer = new ClientDragPlayerPacket()
            {
                TargetId = TargetId,
                RopeSlot = RopeSlotId,
            }
        };
    }
}