using System;
using y1000.Source.Util;

namespace y1000.Source.Item;

public class EquipmentFactory
{
    public static readonly EquipmentFactory Instance = new();

    private readonly ItemSdbReader _itemDb;

    private EquipmentFactory()
    {
        _itemDb = ItemSdbReader.ItemSdb;
    }

    public PlayerWeapon CreatePlayerWeapon(string name, bool male)
    {
        var animationName = _itemDb.GetWeaponSpriteIndex(name, male);
        var attackAni = _itemDb.GetAttackSpriteIndex(name, male);
        var kungfuType = _itemDb.GetAttackKungFuType(name);
        return new PlayerWeapon(name, animationName, attackAni, kungfuType);
    }


    private PlayerChest CreatePlayerChest(string name, bool male, int color )
    {
        var index = _itemDb.GetSpriteIndex(name, male);
        return new PlayerChest(index + "0", index + "1", index + "2", index + "3", index + "4", name, color);
    }

    private PlayerHair CreatePlayerHair(string name, bool male, int color)
    {
        var index = _itemDb.GetSpriteIndex(name, male);
        return new PlayerHair(index + "0", index + "1", index + "2", index + "3", index + "4", name, color);
    }

    private PlayerHat CreatePlayerHat(string name, bool male, int color)
    {
        var index = _itemDb.GetSpriteIndex(name, male);
        return new PlayerHat(index + "0", index + "1", index + "2", index + "3", index + "4", name, color);
    }

    private Boot CreateBoot(string name, bool male, int color = 0)
    {
        var index = _itemDb.GetSpriteIndex(name, male);
        return new Boot(index + "0", index + "1", index + "2", index + "3", index + "4", name, color);
    }


    public Wrist CreateWrist(string name, bool male, int color)
    {
        var index = _itemDb.GetSpriteIndex(name, male, EquipmentType.WRIST_CHESTED);
        var index1 = _itemDb.GetSpriteIndex(name, male, EquipmentType.WRIST);
        return new Wrist(
            index + "0", index + "1", index + "2", index + "3", index + "4",
            index1 + "0", index1 + "1", index1 + "2", index1 + "3", index1 + "4",
            name, color);
    }

    private Clothing CreateClothing(string name, bool male, int color = 0)
    {
        var index = _itemDb.GetSpriteIndex(name, male);
        return new Clothing(index + "0", index + "1", index + "2", index + "3", index + "4", name, color);
    }

    private Trouser CreateTrouser(string name, bool male, int color = 0)
    {
        var index = _itemDb.GetSpriteIndex(name, male);
        return new Trouser(index + "0", index + "1", index + "2", index + "3", index + "4", name, color);
    }

    public T Create<T>(string name, bool male, int color) where T : IEquipment
    {
        return (T)Create(name, male, color);
    }
    
    public IEquipment Create(string name, bool male, int color = 0)
    {
        if (!_itemDb.IsEquipment(name))
        {
            throw new NotSupportedException(name + " is not equipment.");
        }
        var equipmentType = _itemDb.ParseEquipmentType(name);
        return equipmentType switch
        {
            EquipmentType.WEAPON => CreatePlayerWeapon(name, male),
            EquipmentType.CLOTHING => CreateClothing(name, male, color),
            EquipmentType.BOOT => CreateBoot(name, male, color),
            EquipmentType.TROUSER => CreateTrouser(name, male, color),
            EquipmentType.WRIST_CHESTED => CreateWrist(name, male, color),
            EquipmentType.WRIST => CreateWrist(name, male, color),
            EquipmentType.CHEST => CreatePlayerChest(name, male, color),
            EquipmentType.HAIR => CreatePlayerHair(name, male, color),
            EquipmentType.HAT => CreatePlayerHat(name, male, color),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}