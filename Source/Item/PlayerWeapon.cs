using y1000.Source.KungFu.Attack;

namespace y1000.Source.Item;

public class PlayerWeapon 
{
    public PlayerWeapon(string nonAttackAnimation, string attackAnimation, AttackKungFuType attackKungFuType)
    {
        NonAttackAnimation = nonAttackAnimation;
        AttackAnimation = attackAnimation;
        AttackKungFuType = attackKungFuType;
    }

    public string NonAttackAnimation { get; }
    
    public string AttackAnimation { get; }
    
    public AttackKungFuType AttackKungFuType { get; }
    
}