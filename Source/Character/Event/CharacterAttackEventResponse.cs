using Source.Networking.Protobuf;
using y1000.Source.Creature;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character.Event;

public sealed class CharacterAttackEventResponse : IPredictableResponse
{
    private CharacterAttackEventResponse(long sequence, bool accepted, CreatureState? backToState)
    {
        Sequence = sequence;
        Accepted = accepted;
        BackToState = backToState;
    }
    
    public bool Accepted { get; }
    
    public long Sequence { get; }
    
    public CreatureState? BackToState { get; }

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static CharacterAttackEventResponse FromPacket(ClientAttackResponsePacket responsePacket)
    {
        return new CharacterAttackEventResponse(responsePacket.Sequence, responsePacket.Accepted, responsePacket.HasBackToState ? (CreatureState)responsePacket.BackToState : null);
    }

}