using Code.Networking.Gen;

namespace y1000.Source.Input;

public interface IClientEvent
{
    ClientPacket ToPacket();
}