
using System.Collections.Generic;
using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public class QuanFa : AbstractLevelKungFu, IAttackKungFu
{

    private static readonly ISet<string> NAMES = new HashSet<string>()
    {
        "无名拳法",
    };
    
    public static bool Knows(string name)
    {
        return NAMES.Contains(name);
    }


    public CreatureState RandomAttackState()
    {
        return Level < 50 || RANDOM.Next() % 2 == 1 ? CreatureState.FIST : CreatureState.KICK;
    }


    public QuanFa(int level, string name) : base(level, name)
    {
    }
}