using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Character.Event;

public class AttackEntityEvent : IClientEvent
{
    private readonly AttackInput _input;
    private readonly bool _below50;

    public AttackEntityEvent(AttackInput input, bool below50)
    {
        _input = input;
        _below50 = below50;
    }

    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            AttackEventPacket = new AttackEventPacket()
            {
                Below50 = _below50,
                Sequence = _input.Sequence,
                TargetId = _input.Entity.Id
            }
        };
    }
}