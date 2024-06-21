using Source.Networking.Protobuf;

namespace y1000.Source.Networking.Server;

public class ProjectileMessage : IServerMessage
{
    public ProjectileMessage(long shooterId, long targetId, int flyingMillis, int sprite)
    {
        ShooterId = shooterId;
        TargetId = targetId;
        FlyingMillis = flyingMillis;
        SpriteId = sprite;
    }

    public long ShooterId { get; }
    public long TargetId { get; }
    public int FlyingMillis { get; }
    public int SpriteId { get; }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static ProjectileMessage FromPacket(ProjectilePacket packet)
    {
        return new ProjectileMessage(packet.Id, packet.TargetId, packet.FlyingTimeMillis, packet.Sprite);
    }
}