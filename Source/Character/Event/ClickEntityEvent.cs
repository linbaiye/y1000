using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Character.Event;

public class ClickEntityEvent : IClientEvent
{

    private long _clickedId;

    public ClickEntityEvent(long c) {
        _clickedId = c;
    }

    public ClientPacket ToPacket()
    {
        return new ClientPacket() {
            ClickPacket  = new ClickPacket() {
                Id = _clickedId,
            }
        };
    }

}