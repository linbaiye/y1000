using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public class BladeKungFu : AbstractAttackKungFu
{
    public BladeKungFu(int level, string name) : base(level, name)
    {
    }

    protected override CreatureState Above50 => CreatureState.BLADE2H;
    
    protected override CreatureState Below50 => CreatureState.BLADE;

}