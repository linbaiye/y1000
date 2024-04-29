using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Creature;
using y1000.Source.Input;

namespace y1000.Source.Character.Event;

public class AttackEntityEvent : IClientEvent
{
    private readonly AttackInput _input;
    private readonly bool _below50;
    private readonly Direction _direction;

    public AttackEntityEvent(AttackInput input, bool below50, Direction d)
    {
        _input = input;
        _below50 = below50;
        _direction = d;
    }

    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            AttackEventPacket = new ClientAttackEventPacket()
            {
                Below50 = _below50,
                Sequence = _input.Sequence,
                TargetId = _input.Entity.Id,
                Direction = (int)_direction,
            }
        };
    }
}