using y1000.Source.KungFu.Attack;

namespace y1000.Source.Item;

public class PlayerWeapon : IEquipment
{
    public PlayerWeapon(string name, string nonAttackAnimation, string attackAnimation, AttackKungFuType attackKungFuType)
    {
        NonAttackAnimation = nonAttackAnimation;
        AttackAnimation = attackAnimation;
        AttackKungFuType = attackKungFuType;
        Name = name;
    }
    
    public string Name { get; }

    public string NonAttackAnimation { get; }
    
    public string AttackAnimation { get; }
    
    public AttackKungFuType AttackKungFuType { get; }

    public EquipmentType EquipmentType => EquipmentType.WEAPON;
}