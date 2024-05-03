using System;
using Source.Networking.Protobuf;
using y1000.Source.Networking.Server;

namespace y1000.Source.Creature.Event;

public abstract class AbstractCreatureAttackMessage : AbstractEntityMessage
{
    
    public AbstractCreatureAttackMessage(long id, int millisPerSprite, Direction direction) : base(id)
    {
        MillisPerSprite = millisPerSprite;
        Direction = direction;
    }
    
    public int MillisPerSprite { get; }
    
    public Direction Direction { get; }

    public abstract override void Accept(IServerMessageVisitor visitor);

    public static AbstractCreatureAttackMessage FromPacket(CreatureAttackEventPacket packet)
    {
        if (packet.Player) {
            return PlayerAttackMessage.FromPacket(packet);
        }
        else
        {
            throw new Exception();
        }
    }
}