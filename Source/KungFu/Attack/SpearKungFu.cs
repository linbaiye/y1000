using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public class SpearKungFu : AbstractAttackKungFu
{
    public SpearKungFu(int level, string name) : base(level, name)
    {
    }

    protected override CreatureState Above50 => CreatureState.SPEAR;
    protected override CreatureState Below50 => CreatureState.SPEAR;
}