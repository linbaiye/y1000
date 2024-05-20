using System.Collections.Generic;
using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public class SwordKungFu : AbstractLevelKungFu , IAttackKungFu
{
    public SwordKungFu(int level, string name) : base(level, name)
    {
    }
    
    private static readonly ISet<string> NAMES = new HashSet<string>()
    {
        "无名剑法"
    };
        
    public static bool Knows(string name)
    {
        return NAMES.Contains(name);
    }

    public CreatureState RandomAttackState()
    {
        return Level < 50 || RANDOM.Next() % 2 == 1 ? CreatureState.SWORD : CreatureState.SWORD2H;
    }
}