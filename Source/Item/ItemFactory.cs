using System;
using System.Collections.Generic;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking.Server;

namespace y1000.Source.Item;

public class ItemFactory
{

    public static readonly ItemFactory Instance = new ItemFactory();

    private ItemFactory()
    {
    }

    private class WeaponConfig
    {
        public WeaponConfig(string name, int iconId,  string nonAttackAni, string attackAni, AttackKungFuType attackKungFuType)
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
    
    private class ItemConfig
    {
        public ItemConfig(string name, int iconId)
        {
            Name = name;
            IconId = iconId;
        }

        public string Name { get; }
        public int IconId { get; }
    }

    private static readonly List<WeaponConfig> WeaponConfigs = new List<WeaponConfig>()
    {
        new WeaponConfig("长剑", 1, "j10", "j12", AttackKungFuType.SWORD),
        new WeaponConfig("木弓", 4, "j210", "j214", AttackKungFuType.BOW),
        new WeaponConfig("长刀", 1, "j10", "j12", AttackKungFuType.BLADE)
    };

    private static readonly List<ItemConfig> ItemConfigs = new List<ItemConfig>()
    {
        new ItemConfig("箭", 60),
    };

    public PlayerWeapon CreatePlayerWeapon(string name)
    {

        return CreateWeapon(name,
            config => new PlayerWeapon(config.NonAttackAni, config.AttackAni, config.AttackKungFuType));
    }


    private T CreateWeapon<T>(string name, Func<WeaponConfig, T> creator)
    {
        foreach (var config in WeaponConfigs)
        {
            if (name.Equals(config.Name))
            {
                return creator.Invoke(config);
            }
        }
        throw new NotSupportedException("Not supported name " + name);
    }

    private CharacterStackItem CreateItem(string name, int number)
    {
        foreach (var itemConfig in ItemConfigs)
        {
            if (itemConfig.Name.Equals(name))
            {
                return new CharacterStackItem(itemConfig.IconId, name, number);
            }
        }
        throw new NotSupportedException("Not supported name " + name);
    }


    public ICharacterItem CreateCharacterItem(JoinedRealmMessage.InventoryItemMessage message)
    {
        if (message.Type == ItemType.WEAPON)
        {
            return CreateCharacterWeapon(message.Name, message.Type);
        }
        else if (message.Type == ItemType.STACK)
        {
            return CreateItem(message.Name, message.Number);
        }
        throw new NotSupportedException("Not supported name " + message.Name);
    }

    public ICharacterItem CreateCharacterWeapon(string name, ItemType type)
    {
        if (type == ItemType.WEAPON)
        {
            return CreateWeapon<CharacterWeapon>(name, config => new CharacterWeapon(name, config.IconId, config.NonAttackAni, config.AttackAni, config.AttackKungFuType));
        }
        throw new NotSupportedException("Not supported type " + type);
    }
}