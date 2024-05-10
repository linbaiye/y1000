using System;
using y1000.Source.Entity;
using y1000.Source.Input;
using y1000.Source.Sprite;

namespace y1000.Source.KungFu.Attack;

public interface IAttackKungFu : ILevelKungFu
{
    void Attack(Character.Character character, AttackInput input);

    public static IAttackKungFu? ByName(string name, int level)
    {
        if (QuangFa.Knows(name))
        {
            return new QuangFa(level, name);
        }
        return null;
    }
}