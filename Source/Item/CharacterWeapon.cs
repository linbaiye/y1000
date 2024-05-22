using y1000.Source.KungFu.Attack;

namespace y1000.Source.Item;

public class CharacterWeapon : PlayerWeapon, ICharacterItem
{
    public int IconId { get; }
    
    public string Name { get; }

    public CharacterWeapon(string name, int iconId, string nonAttackAnimation, string attackAnimation, AttackKungFuType attackKungFuType) : base(nonAttackAnimation, attackAnimation, attackKungFuType)
    {
        IconId = iconId;
        Name = name;
    }
}