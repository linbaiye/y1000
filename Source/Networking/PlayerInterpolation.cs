using Godot;
using NLog;
using Source.Networking.Protobuf;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class PlayerInterpolation : IServerMessage
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    private PlayerInterpolation(Interpolation interpolation,  PlayerInfo playerInfo)
    {
        Interpolation = interpolation;
        PlayerInfo = playerInfo;
    }

    public Interpolation Interpolation { get; }
    
    public PlayerInfo PlayerInfo { get; }
    

    public static PlayerInterpolation FromPacket(PlayerInterpolationPacket packet)
    {
        return new PlayerInterpolation(Interpolation.FromPacket(packet.Interpolation),
            PlayerInfo.FromPacket(packet.Info));
    }

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}