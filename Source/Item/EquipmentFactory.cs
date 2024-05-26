namespace y1000.Source.Item;

public class EquipmentFactory
{
    public static readonly EquipmentFactory Instance = new EquipmentFactory();

    private readonly ItemDb itemDb;

    private EquipmentFactory()
    {
        itemDb = ItemDb.Instance;
    }

    public PlayerWeapon CreatePlayerWeapon(string name, bool male)
    {
        var animationName = itemDb.GetAnimationName(name, male);
        var attackAni = itemDb.GetAttackAnimationName(name, male);
        var kungfuType = itemDb.GetAttackKungFuType(name);
        return new PlayerWeapon(animationName, attackAni, kungfuType);
    }
    
    
    public CharacterWeapon CreateCharacterWeapon(string name, bool male)
    {
        var animationName = itemDb.GetAnimationName(name, male);
        var attackAni = itemDb.GetAttackAnimationName(name, male);
        var kungfuType = itemDb.GetAttackKungFuType(name);
        return new CharacterWeapon(name, animationName, attackAni, kungfuType);
    }
}