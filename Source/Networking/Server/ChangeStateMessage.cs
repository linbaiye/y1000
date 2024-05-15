using Source.Networking.Protobuf;
using y1000.Source.Creature;

namespace y1000.Source.Networking.Server;

public class ChangeStateMessage : AbstractEntityMessage
{
    private ChangeStateMessage(long id, CreatureState newState) : base(id)
    {
        NewState = newState;
    }
    public CreatureState NewState { get; }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static ChangeStateMessage FromPacket(ChangeStatePacket packet)
    {
        return new ChangeStateMessage(packet.Id, (CreatureState)packet.State);
    }
}