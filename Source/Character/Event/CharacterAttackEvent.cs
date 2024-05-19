using Source.Networking.Protobuf;
using y1000.Source.Creature;
using y1000.Source.Input;

namespace y1000.Source.Character.Event;

public class CharacterAttackEvent : IClientEvent
{
    private readonly AttackInput _input;
    private readonly CreatureState _state;
    private readonly Direction _direction;

    public CharacterAttackEvent(AttackInput input, CreatureState state, Direction d)
    {
        _input = input;
        _state = state;
        _direction = d;
    }

    public CreatureState State => _state;

    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            AttackEventPacket = new ClientAttackEventPacket()
            {
                State = (int)_state,
                Sequence = _input.Sequence,
                TargetId = _input.Entity.Id,
                Direction = (int)_direction,
            }
        };
    }
}