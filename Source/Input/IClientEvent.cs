using Source.Networking.Protobuf;

namespace y1000.Source.Input;

public interface IClientEvent
{
    ClientPacket ToPacket();
}