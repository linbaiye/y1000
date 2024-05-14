using Godot;
using NLog;
using Source.Networking.Protobuf;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class PlayerInterpolation : IServerMessage
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
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
        return $"{nameof(Interpolation)}: {Interpolation}, {nameof(Id)}: {Id}, {nameof(Male)}: {Male}";
    }

    public static PlayerInterpolation FromPacket(PlayerInterpolationPacket packet)
    {
        return new PlayerInterpolation(Interpolation.FromPacket(packet.Interpolation), packet.Male, packet.Id);
    }

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}