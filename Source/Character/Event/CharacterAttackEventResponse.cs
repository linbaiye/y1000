using Source.Networking.Protobuf;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character.Event;

public sealed class CharacterAttackEventResponse : IPredictableResponse
{
    
    public CharacterAttackEventResponse(long sequence, bool accepted)
    {
        Sequence = sequence;
        Accepted = accepted;
    }
    
    public bool Accepted { get; }
    
    public long Sequence { get; }

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static CharacterAttackEventResponse FromPacket(ClientAttackResponsePacket responsePacket)
    {
        return new CharacterAttackEventResponse(responsePacket.Sequence, responsePacket.Accepted);
    }

}