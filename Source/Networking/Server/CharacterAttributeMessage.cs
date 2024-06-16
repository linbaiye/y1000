using Source.Networking.Protobuf;
using y1000.Source.Character;
using y1000.Source.Util;

namespace y1000.Source.Networking.Server;

public class CharacterAttributeMessage : ICharacterMessage
{
    private CharacterAttributeMessage(ValueBar health, ValueBar power, ValueBar innerPower, ValueBar outerPower)
    {
        Health = health;
        Power = power;
        InnerPower = innerPower;
        OuterPower = outerPower;
    }

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public ValueBar Health { get; }
    public ValueBar Power{ get; }
    public ValueBar InnerPower{ get; }
    public ValueBar OuterPower{ get; }
    
    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static CharacterAttributeMessage FromPacket(AttributePacket packet)
    {
        return new CharacterAttributeMessage(
            new ValueBar(packet.CurLife, packet.MaxLife),
            new ValueBar(packet.CurPower, packet.MaxPower),
            new ValueBar(packet.CurInnerPower, packet.MaxInnerPower),
            new ValueBar(packet.CurOuterPower, packet.MaxOuterPower));
    }
}