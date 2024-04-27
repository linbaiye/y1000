using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class PlayerInterpolation : IServerMessage
{
    private PlayerInterpolation(Interpolation interpolation, bool male, long id)
    {
        Interpolation = interpolation;
        Male = male;
        Id = id;
    }

    public Interpolation Interpolation { get; }
    
    public long Id { get; }
    
    public bool Male { get; }

    public override string ToString()
    {
        return $"{nameof(Interpolation)}: {Interpolation}";
    }

    public static PlayerInterpolation FromPacket(PlayerInterpolationPacket packet)
    {
        return new PlayerInterpolation(Interpolation.FromPacket(packet.Interpolation), packet.Male, packet.Id);
    }

    public void HandleBy(IServerMessageHandler handler)
    {
        handler.Handle(this);
    }
}