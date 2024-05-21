
using y1000.Source.KungFu.Attack;

namespace y1000.Source.Item;

public class Weapon : IItem
{
    public Weapon(string itemName, int iconId, string nonAttackAnimation, string attackAnimation, AttackKungFuType attackKungFuType)
    {
        ItemName = itemName;
        IconId = iconId;
        AttackAnimation = attackAnimation;
        NonAttackAnimation = nonAttackAnimation;
        AttackKungFuType = attackKungFuType;
    }

    public string ItemName { get; }
    
    public int IconId { get; }
    
    public string AttackAnimation { get; }
    
    public string NonAttackAnimation { get; }
    
    public AttackKungFuType AttackKungFuType { get; }
    

    public ItemType Type => ItemType.WEAPON;

}