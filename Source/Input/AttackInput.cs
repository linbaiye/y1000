using y1000.Source.Entity;

namespace y1000.Source.Input;


public class AttackInput : AbstractPredictableInput
{
    public AttackInput(long s, IEntity entity) : base(s)
    {
        Entity = entity;
    }

    public IEntity Entity { get; }
    
    public override InputType Type => InputType.ATTACK;
}