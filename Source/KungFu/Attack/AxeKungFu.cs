using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public class AxeKungFu : AbstractAttackKungFu
{
    public AxeKungFu(int level, string name) : base(level, name)
    {
    }

    protected override CreatureState Above50 => CreatureState.AXE;
    protected override CreatureState Below50 => CreatureState.AXE;
}