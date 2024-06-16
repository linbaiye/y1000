using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public class ThrowKungFu : AbstractAttackKungFu
{
    public ThrowKungFu(int level, string name) : base(level, name)
    {
    }

    protected override CreatureState Above50 { get; }
    protected override CreatureState Below50 { get; }
}