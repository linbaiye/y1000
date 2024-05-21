using y1000.Source.KungFu.Attack;

namespace y1000.Source.Item;

public class CharacterWeapon : PlayerWeapon, ICharacterItem
{
    public long Id { get; }

    public long IconId { get; }
    
    public string Name { get; }

    public CharacterWeapon(long id, string name, long iconId, string nonAttackAnimation, string attackAnimation, AttackKungFuType attackKungFuType) : base(nonAttackAnimation, attackAnimation, attackKungFuType)
    {
        Id = id;
        IconId = iconId;
        Name = name;
    }
}