using Source.Networking.Protobuf;
using y1000.Source.Entity;
using y1000.Source.Input;

namespace y1000.Source.Character.Event;

public class AttackInput : AbstractPredictableInput
{
    public AttackInput(long s, IEntity entity) : base(s)
    {
        Entity = entity;
    }

    public IEntity Entity { get; }
    
    public override InputType Type => InputType.ATTACK;
}