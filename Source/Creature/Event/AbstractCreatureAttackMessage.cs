using System;
using Source.Networking.Protobuf;
using y1000.Source.Networking.Server;

namespace y1000.Source.Creature.Event;

public abstract class AbstractCreatureAttackMessage : AbstractEntityMessage
{
    protected AbstractCreatureAttackMessage(long id, Direction direction, CreatureState state) : base(id)
    {
        Direction = direction;
        State = state;
    }
    
    public Direction Direction { get; }
    
    public CreatureState State { get; }

    public abstract override void Accept(IServerMessageVisitor visitor);

    public static AbstractCreatureAttackMessage FromPacket(CreatureAttackEventPacket packet)
    {
        if (packet.Player)
        {
            return new PlayerAttackMessage(packet.Id, (Direction)packet.Direction, (CreatureState)packet.State);
        }
        return new CreatureAttackMessage(packet.Id, (Direction)packet.Direction, (CreatureState)packet.State);
    }
}