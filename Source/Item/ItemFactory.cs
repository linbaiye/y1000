using System;
using System.Collections.Generic;
using y1000.Source.KungFu.Attack;

namespace y1000.Source.Item;

public class ItemFactory
{

    public static readonly ItemFactory Instance = new ItemFactory();

    private ItemFactory()
    {
    }

    private class ItemConfig
    {
        public ItemConfig(string name, int iconId,  string nonAttackAni, string attackAni, AttackKungFuType attackKungFuType)
        {
            Name = name;
            IconId = iconId;
            AttackAni = attackAni;
            NonAttackAni = nonAttackAni;
            AttackKungFuType = attackKungFuType;
        }

        public string Name { get; }
        
        public int IconId { get; }
        
        public string AttackAni { get; }
        
        public string NonAttackAni { get; }
        
        public AttackKungFuType AttackKungFuType { get; }
        
    }

    private static readonly List<ItemConfig> WeaponConfigs = new List<ItemConfig>()
    {
        new ItemConfig("长剑", 1, "j10", "j12", AttackKungFuType.SWORD),
        new ItemConfig("木弓", 4, "j210", "j214", AttackKungFuType.BOW)
    };

    public PlayerWeapon CreatePlayerWeapon(string name)
    {
        foreach (var config in WeaponConfigs)
        {
            if (name.Equals(config.Name))
            {
                return new PlayerWeapon( config.NonAttackAni, config.NonAttackAni, config.AttackKungFuType);
            }
        }
        throw new NotSupportedException("Not supported name " + name);
    }


    private T CreateWeapon<T>(string name, Func<>)
    {
        
    }
    

    public ICharacterItem CreateCharacterItem(long id, string name, ItemType type)
    {
        if (type == ItemType.WEAPON)
        {
            return new CharacterWeapon(id,  name, iconId, config.NonAttackAni, config.NonAttackAni, config.AttackKungFuType);
        }
        throw new NotSupportedException();
    }
}