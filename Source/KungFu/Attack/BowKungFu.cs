using System.Collections.Generic;
using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public class BowKungFu : AbstractLevelKungFu, IAttackKungFu
{
    private static readonly ISet<string> NAMES = new HashSet<string>()
    {
        "无名弓术"
    };
        
    public static bool Knows(string name)
    {
        return NAMES.Contains(name);
    }

    public CreatureState RandomAttackState() => CreatureState.BOW;

    public BowKungFu(int level, string name) : base(level, name)
    {
    }
}