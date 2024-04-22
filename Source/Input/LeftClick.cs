using Godot;
using Source.Networking.Protobuf;

namespace y1000.Source.Input;

public class LeftClick: IInput
{
    public LeftClick(long id)
    {
        Id = id;
    }
    
    public long Id { get; }
    public InputType Type => InputType.LEFT_CLICK;

    public InputPacket ToPacket()
    {
        throw new System.NotImplementedException();
    }
}