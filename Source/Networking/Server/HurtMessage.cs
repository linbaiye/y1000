using Source.Networking.Protobuf;

namespace y1000.Source.Networking.Server;

public class HurtMessage : AbstractEntityMessage
{
    public HurtMessage(long id) : base(id)
    {
    }
    
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static HurtMessage FromPacket(CreatureHurtEventPacket packet)
    {
        return new HurtMessage(packet.Id);
    }
}