using System.Collections.Generic;
using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public class SwordKungFu : AbstractAttackKungFu
{
    public SwordKungFu(int level, string name) : base(level, name)
    {
    }


    protected override CreatureState Above50 => CreatureState.SWORD2H;
    
    protected override CreatureState Below50 => CreatureState.SWORD;
}