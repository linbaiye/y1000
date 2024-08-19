using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Character.Event;

public class ClickEntityEvent : IInput
{
    public ClientPacket ToPacket()
    {
        throw new System.NotImplementedException();
    }

    public InputType Type => InputType.LEFT_CLICK;
}