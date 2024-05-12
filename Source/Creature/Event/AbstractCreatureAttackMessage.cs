using System;
using Source.Networking.Protobuf;
using y1000.Source.Networking.Server;

namespace y1000.Source.Creature.Event;

public abstract class AbstractCreatureAttackMessage : AbstractEntityMessage
{
    public AbstractCreatureAttackMessage(long id, Direction direction) : base(id)
    {
        Direction = direction;
    }
    
    public Direction Direction { get; }

    public abstract override void Accept(IServerMessageVisitor visitor);

    public static AbstractCreatureAttackMessage FromPacket(CreatureAttackEventPacket packet)
    {
        if (packet.Player)
        {
            return new PlayerAttackMessage(packet.Id, packet.Below50, (Direction)packet.Direction);
        }
        return new CreatureAttackMessage(packet.Id, (Direction)packet.Direction);
    }
}