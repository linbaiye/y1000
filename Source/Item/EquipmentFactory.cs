using System;

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
        return new PlayerWeapon(name, animationName, attackAni, kungfuType);
    }
    

    public PlayerChest CreatePlayerChest(string name, bool male)
    {
        var index = itemDb.GetSpriteIndex(name, male);
        return new PlayerChest(index + "0", index + "1", index + "2", index + "3", index + "4", name);
    }

    public PlayerHair CreatePlayerHair(string name, bool male)
    {
        var index = itemDb.GetSpriteIndex(name, male);
        return new PlayerHair(index + "0", index + "1", index + "2", index + "3", index + "4", name);
    }
    
    public PlayerHat CreatePlayerHat(string name, bool male)
    {
        var index = itemDb.GetSpriteIndex(name, male);
        return new PlayerHat(index + "0", index + "1", index + "2", index + "3", index + "4", name);
    }
    
    public Boot CreateBoot(string name, bool male)
    {
        var index = itemDb.GetSpriteIndex(name, male);
        return new Boot(index + "0", index + "1", index + "2", index + "3", index + "4", name);
    }


    public Wrist CreateWrist(string name, bool male, bool chested)
    {
        var index = itemDb.GetSpriteIndex(name, male, chested ? EquipmentType.WRIST_CHESTED : EquipmentType.WRIST);
        return new Wrist(index + "0", index + "1", index + "2", index + "3", index + "4", name);
    }
    
    public Clothing CreateClothing(string name, bool male)
    {
        var index = itemDb.GetSpriteIndex(name, male);
        return new Clothing(index + "0", index + "1", index + "2", index + "3", index + "4", name);
    }
    
    public Trouser CreateTrouser(string name, bool male)
    {
        var index = itemDb.GetSpriteIndex(name, male);
        return new Trouser(index + "0", index + "1", index + "2", index + "3", index + "4", name);
    }
    
    public IEquipment Create(string name, bool male)
    {
        if (!itemDb.IsEquipment(name))
        {
            throw new NotSupportedException(name + " is not equipment.");
        }
        var equipmentType = itemDb.ParseEquipmentType(name);
        return equipmentType switch
        {
            EquipmentType.WEAPON => CreatePlayerWeapon(name, male),
            EquipmentType.CLOTHING => CreateClothing(name, male),
            EquipmentType.BOOT => CreateBoot(name, male),
            EquipmentType.TROUSER => CreateTrouser(name, male),
            EquipmentType.WRIST_CHESTED => CreateWrist(name, male, true),
            EquipmentType.WRIST => CreateWrist(name, male, false),
            EquipmentType.CHEST => CreatePlayerChest(name, male),
            EquipmentType.HAIR => CreatePlayerHair(name, male),
            EquipmentType.HAT => CreatePlayerHat(name, male),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}