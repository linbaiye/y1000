
using System.Collections.Generic;
using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public class QuanFa : AbstractAttackKungFu
{

    public QuanFa(int level, string name) : base(level, name)
    {
    }

    protected override CreatureState Above50 => CreatureState.KICK;
    
    protected override CreatureState Below50 => CreatureState.FIST;
}