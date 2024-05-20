using System;
using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public interface IAttackKungFu : ILevelKungFu
{
    public CreatureState RandomAttackState();
    
    public static IAttackKungFu ByName(string name, int level)
    {
        if (QuanFa.Knows(name))
        {
            return new QuanFa(level, name);
        }
        if (BowKungFu.Knows(name))
        {
            return new BowKungFu(level, name);
        }
        if (SwordKungFu.Knows(name))
        {
            return new SwordKungFu(level, name);
        }
        throw new NotImplementedException();
    }
    
    public static readonly IAttackKungFu Empty = EmptyAttackKungFu.Instance;
    
}