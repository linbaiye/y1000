using System;
using y1000.Source.Item;
using y1000.Source.KungFu.Attack;

namespace y1000.Source.Util;

public class ItemSdbReader : AbstractSdbReader
{

    public static readonly ItemSdbReader ItemSdb = Load();

    private ItemSdbReader()
    {
    }

    public bool CanStack(string item)
    {
        return Parse(item, "boDouble", s => s.Equals("TRUE"));
    }

    public AttackKungFuType GetAttackKungFuType(string weaponName)
    {
        return Parse(weaponName, "HitType", str => (AttackKungFuType)int.Parse(str));
    }


    public int GetIconId(string itemName)
    {
        return Parse(itemName, "Shape", int.Parse);
    }

    public int GetColor(string itemName)
    {
        return string.IsNullOrEmpty(GetString(itemName, "Color")) ? 0:
            Parse(itemName, "Color", int.Parse);
    }
    
    public string GetAttackSpriteIndex(string equip, bool male)
    {
        return ParseEquipmentAnimation(equip, male, () => Parse(equip, "HitMotion", s=> s));
    }

    public int GetPrice(string itemName)
    {
        return Parse(itemName, "Price", int.Parse);
    }

    public bool IsEquipment(string name)
    {
        var kind = Parse(name, "Kind", s => s);
        return kind.Equals("6") || kind.Equals("24");
    }
    
    private string ParseEquipmentAnimation(string equip, EquipmentType type, bool male, Func<string> postfix)
    {
        if (!IsEquipment(equip))
        {
            throw new NotImplementedException(equip + " is not equipment.");
        }
        if (male)
        {
            return type == EquipmentType.WEAPON ? 
                "w" + Parse(equip, "WearShape", s => s) + postfix.Invoke()
                : Convert.ToChar('n' + (int)type) + Parse(equip, "WearShape", s => s);
        }
        return type == EquipmentType.WEAPON ? 
            "j" + Parse(equip, "WearShape", s => s) + postfix.Invoke()
            : Convert.ToChar('a' + (int)type) + Parse(equip, "WearShape", s => s);
    }

    private string ParseEquipmentAnimation(string equip, bool male, Func<string> postfix)
    {
        var type = ParseEquipmentType(equip);
        return ParseEquipmentAnimation(equip, type, male, postfix);
    }

    public EquipmentType ParseEquipmentType(string item)
    {
        return Parse(item, "WearPos", s => (EquipmentType)int.Parse(s));
    }

    public string GetWeaponSpriteIndex(string equip, bool male)
    {
        return ParseEquipmentAnimation(equip, male, () => "0");
    }
    
    public string GetSpriteIndex(string equip, bool male)
    {
        return ParseEquipmentAnimation(equip, male, () => "");
    }
    
    public string GetSpriteIndex(string equip, bool male, EquipmentType type)
    {
        return ParseEquipmentAnimation(equip, type, male, () => "");
    }


    private static ItemSdbReader Load()
    {
        var itemSdbReader = new ItemSdbReader();
        itemSdbReader.Read("res://assets/sdb/Item.sdb");
        return itemSdbReader;
    }
}