
using System;
using System.Collections.Generic;
using Godot;
using y1000.Source.KungFu.Attack;

namespace y1000.Source.Item;

public class ItemDb
{

    public static readonly ItemDb Instance = Load();

    private readonly Dictionary<string, int> _header;
    private readonly Dictionary<string, string[]> _items;

    private ItemDb(Dictionary<string, string[]> items, Dictionary<string, int> header)
    {
        _items = items;
        _header = header;
    }

    private T Parse<T>(string itemName, string key, Func<string, T> creator)
    {
        if (!_items.TryGetValue(itemName, out var item))
        {
            throw new NotImplementedException();
        }

        if (!_header.TryGetValue(key, out var index))
        {
            throw new NotImplementedException();
        }

        return creator.Invoke(item[index]);
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

    public string GetAttackAnimationName(string equip, bool male)
    {
        return ParseEquipmentAnimation(equip, male, () => Parse(equip, "HitMotion", s=> s));
    }

    private string ParseEquipmentAnimation(string equip, bool male, Func<string> postfix)
    {
        var kind = Parse(equip, "Kind", s => s);
        if (!kind.Equals("6"))
        {
            throw new NotImplementedException(equip + " is not equipment.");
        }
        var type = ParseEquipmentType(equip);
        if (male)
        {
            return type == EquipmentType.WEAPON ? 
                "w" + Parse(equip, "WearShape", s => s) + postfix.Invoke()
                : "n" + (int)type;
        }
        return type == EquipmentType.WEAPON ? 
            "j" + Parse(equip, "WearShape", s => s) + postfix.Invoke()
            : "a" + (int)type;
    }

    private EquipmentType ParseEquipmentType(string item)
    {
        return Parse(item, "WearPos", s => (EquipmentType)int.Parse(s));
    }

    public string GetAnimationName(string equip, bool male)
    {
        return ParseEquipmentAnimation(equip, male, () => "0");
    }

    private static ItemDb Load()
    {
        var filepath = "res://assets/sdb/Item.sdb";
        var fileAccess = FileAccess.Open(filepath, FileAccess.ModeFlags.Read);
        var line = fileAccess.GetLine();
        if (line == null)
        {
            throw new NotImplementedException("Item.sdb does not have header.");
        }
        var headers = line.Split(",");
        Dictionary<string, int> header = new Dictionary<string, int>();
        for (int i = 0; i < headers.Length; i++)
        {
            header.TryAdd(headers[i], i);
        }

        Dictionary<string, string[]> items = new Dictionary<string, string[]>();
        while ((line = fileAccess.GetLine()) != null)
        {
            if (string.IsNullOrEmpty(line))
            {
                break;
            }
            var strings = line.Split(",");
            if (!string.IsNullOrEmpty(strings[0]))
            {
                items.Add(strings[0], strings);
            }
        }
        return new ItemDb(items, header);
    }
}