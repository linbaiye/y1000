using Source.Networking.Protobuf;

namespace y1000.Source.Networking.Server;

public class PlayerProjectileMessage : IServerMessage
{
    public PlayerProjectileMessage(long playerId, long targetId, int flyingMillis)
    {
        PlayerId = playerId;
        TargetId = targetId;
        FlyingMillis = flyingMillis;
    }

    public long PlayerId { get; }
    public long TargetId { get; }
    public int FlyingMillis { get; }
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static PlayerProjectileMessage FromPacket(PlayerProjectilePacket packet)
    {
        return new PlayerProjectileMessage(packet.Id, packet.TargetId, packet.FlyingTimeMillis);
    }
}