using Godot;
using NLog;
using Source.Networking.Protobuf;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class PlayerInterpolation : IServerMessage
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    private PlayerInterpolation(Interpolation interpolation, bool male, long id, string name)
    {
        Interpolation = interpolation;
        Male = male;
        Id = id;
        Name = name;
    }

    public Interpolation Interpolation { get; }
    
    public long Id { get; }
    
    public bool Male { get; }
    
    public string Name { get; }
    
    public string? WeaponName { get; private set; }

    public override string ToString()
    {
        return $"{nameof(Interpolation)}: {Interpolation}, {nameof(Id)}: {Id}, {nameof(Male)}: {Male}";
    }

    public static PlayerInterpolation FromPacket(PlayerInterpolationPacket packet)
    {
        var i = new PlayerInterpolation(Interpolation.FromPacket(packet.Interpolation), packet.Male, packet.Id, packet.Name);
        if (packet.HasWeaponName)
        {
            i.WeaponName = packet.WeaponName;
        }

        return i;
    }

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}