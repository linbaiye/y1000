using Source.Networking.Protobuf;
using y1000.Source.Entity;
using y1000.Source.Input;

namespace y1000.Source.Character.Event;

public class AttackEntityInput : IPredictableInput
{
    public AttackEntityInput(long s, IEntity entity)
    {
        Entity = entity;
        Sequence = s;
    }

    public IEntity Entity { get; }

    public long Sequence { get; }
    
    InputPacket IPredictableInput.ToPacket()
    {
        throw new System.NotImplementedException();
    }

    public ClientPacket ToPacket()
    {
        throw new System.NotImplementedException();
    }

    public InputType Type => InputType.ATTACK;
}