
using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public class BowKungFu : AbstractAttackKungFu
{
    public BowKungFu(int level, string name) : base(level, name)
    {
    }

    protected override CreatureState Above50 => CreatureState.BOW;
    
    protected override CreatureState Below50 => CreatureState.BOW;
}