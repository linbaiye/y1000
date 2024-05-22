using System;
using y1000.Source.Creature;

namespace y1000.Source.KungFu.Attack;

public interface IAttackKungFu : ILevelKungFu
{
    public CreatureState RandomAttackState();
    
    public static IAttackKungFu ByType(AttackKungFuType type, string name, int level)
    {
        return type switch
        {
            AttackKungFuType.BLADE => new BladeKungFu(level, name),
            AttackKungFuType.SWORD => new SwordKungFu(level, name),
            AttackKungFuType.BOW => new BowKungFu(level, name),
            AttackKungFuType.QUANFA => new QuanFa(level, name),
            _ => throw new NotImplementedException("Type " + type + " not supported.")
        };
    }
    
    public static readonly IAttackKungFu Empty = EmptyAttackKungFu.Instance;
    
}