using y1000.Source.KungFu.Attack;

namespace y1000.Source.Item;

public class CharacterWeapon : PlayerWeapon
{
    public CharacterWeapon(string name, string nonAttackAnimation, string attackAnimation, AttackKungFuType attackKungFuType) : base(nonAttackAnimation, attackAnimation, attackKungFuType)
    {
        ItemName = name;
    }

    public string ItemName { get; }
}