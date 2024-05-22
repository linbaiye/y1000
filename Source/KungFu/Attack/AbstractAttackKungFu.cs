using System;
using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public abstract class AbstractAttackKungFu : AbstractLevelKungFu, IAttackKungFu
{
    private static readonly Random RANDOM = new();
    protected AbstractAttackKungFu(int level, string name) : base(level, name)
    {
    }

    protected abstract CreatureState Above50 { get; }
    
    protected abstract CreatureState Below50 { get; }
    
    public CreatureState RandomAttackState()
    {
        return Level < 50 || RANDOM.Next() % 2 == 1 ? Below50 : Above50;
    }
}