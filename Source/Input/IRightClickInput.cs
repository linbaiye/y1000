using Source.Networking.Protobuf;

namespace y1000.Source.Input;

public interface IRightClickInput : IPredictableInput
{
    InputPacket ToRightClickPacket();
}