using Godot;
using Source.Networking.Protobuf;
using y1000.code;

namespace y1000.Source.Networking;

public class PlayerInterpolation : IServerMessage
{
    private PlayerInterpolation(Interpolation interpolation, bool male)
    {
        Interpolation = interpolation;
        Male = male;
    }

    public Interpolation Interpolation { get; }
    
    public bool Male { get; }

    public static PlayerInterpolation FromPacket(PlayerInterpolationPacket packet)
    {
        return new PlayerInterpolation(Interpolation.FromPacket(packet.Interpolation), packet.Male);
    }

    public void Accept(IServerMessageHandler handler)
    {
        handler.Handle(this);
    }
}