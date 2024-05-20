
namespace y1000.Source.Item.Weapon;

public class Weapon : IItem
{
    public Weapon(string itemName, int shapeId, string animationId, WeaponType type)
    {
        ItemName = itemName;
        ShapeId = shapeId;
        AnimationId = animationId;
        Type = type;
    }

    public string ItemName { get; }
    
    public int ShapeId { get; }
    
    public string AnimationId { get; }
    
    public WeaponType Type { get; }

    public static Weapon SWORD = new Weapon("长剑", 1, "j10", WeaponType.SWORD);
}