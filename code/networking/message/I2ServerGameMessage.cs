using Code.Networking.Gen;

namespace y1000.code.networking.message
{
    public interface I2ServerGameMessage 
    {
        Packet ToPacket();
    }
}