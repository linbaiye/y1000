namespace y1000.Source.Item;

public class EquipmentFactory
{
    public static readonly EquipmentFactory Instance = new();

    private readonly ItemDb itemDb;

    private EquipmentFactory()
    {
        itemDb = ItemDb.Instance;
    }

    public PlayerWeapon CreatePlayerWeapon(string name, bool male)
    {
        var animationName = itemDb.GetWeaponSpriteIndex(name, male);
        var attackAni = itemDb.GetAttackSpriteIndex(name, male);
        var kungfuType = itemDb.GetAttackKungFuType(name);
        return new PlayerWeapon(animationName, attackAni, kungfuType);
    }
    
    
    public CharacterWeapon CreateCharacterWeapon(string name, bool male)
    {
        var animationName = itemDb.GetWeaponSpriteIndex(name, male);
        var attackAni = itemDb.GetAttackSpriteIndex(name, male);
        var kungfuType = itemDb.GetAttackKungFuType(name);
        return new CharacterWeapon(name, animationName, attackAni, kungfuType);
    }

    public PlayerChest CreatePlayerChest(string name, bool male)
    {
        var index = itemDb.GetSpriteIndex(name, male);
        return new PlayerChest(index + "0", index + "1", index + "2", index + "3", index + "4", name);
    }
}