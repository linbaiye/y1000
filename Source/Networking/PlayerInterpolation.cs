using Godot;
using NLog;
using Source.Networking.Protobuf;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class PlayerInterpolation : IServerMessage
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    private PlayerInterpolation(Interpolation interpolation, bool male, long id, AttackKungFuType attackKungFuType,
        bool attackKungFuBelow50,
        int kungFuSpriteMillis)
    {
        Interpolation = interpolation;
        Male = male;
        Id = id;
        AttackKungFuType = attackKungFuType;
        AttackKungFuBelow50 = attackKungFuBelow50;
        KungFuSpriteMillis = kungFuSpriteMillis;
    }

    public Interpolation Interpolation { get; }
    
    public long Id { get; }
    
    public bool Male { get; }

    public AttackKungFuType AttackKungFuType { get; }
    
    public bool AttackKungFuBelow50 { get; }
    
    public int KungFuSpriteMillis { get; }

    public override string ToString()
    {
        return $"{nameof(Interpolation)}: {Interpolation}, {nameof(Id)}: {Id}, {nameof(Male)}: {Male}";
    }

    public static PlayerInterpolation FromPacket(PlayerInterpolationPacket packet)
    {
        return new PlayerInterpolation(Interpolation.FromPacket(packet.Interpolation), packet.Male, packet.Id, 
            (AttackKungFuType)packet.KungFuType, packet.KungFuBelow50,
            packet.KungFuSpriteMillis);
    }

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}