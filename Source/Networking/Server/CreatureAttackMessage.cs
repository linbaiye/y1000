using Source.Networking.Protobuf;
using y1000.Source.Creature;

namespace y1000.Source.Networking.Server;

public sealed class CreatureAttackMessage : AbstractEntityMessage
{
    public CreatureAttackMessage(long id, bool below50, Direction direction, int spriteMillis) : base(id)
    {
        Below50 = below50;
        Direction = direction;
        SpriteMillis = spriteMillis;
    }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public bool Below50 { get; }
    
    public Direction Direction { get; }
    
    public int SpriteMillis { get; }

    public static CreatureAttackMessage FromPacket(CreatureAttackEventPacket attackEventPacket)
    {
        return new CreatureAttackMessage(attackEventPacket.Id, attackEventPacket.Below50, (Direction)attackEventPacket.Direction, attackEventPacket.SpriteMillis);
    }
}