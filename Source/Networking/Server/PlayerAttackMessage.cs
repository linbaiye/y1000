using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Creature;
using y1000.Source.Creature.Event;

namespace y1000.Source.Networking.Server;

public sealed class PlayerAttackMessage : AbstractCreatureAttackMessage
{
    public PlayerAttackMessage(long id, bool below50, Direction direction, int spriteMillis) : base(id, spriteMillis, direction)
    {
        Below50 = below50;
    }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public bool Below50 { get; }
    
    public static PlayerAttackMessage FromPacket(CreatureAttackEventPacket attackEventPacket)
    {
        return new PlayerAttackMessage(attackEventPacket.Id, attackEventPacket.Below50, (Direction)attackEventPacket.Direction, attackEventPacket.SpriteMillis);
    }
}