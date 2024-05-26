using System;
using y1000.Source.Item;

namespace y1000.Source.Character.Event;

public class WeaponChangedEvent : EventArgs
{
    public WeaponChangedEvent(CharacterWeapon? weapon)
    {
        Weapon = weapon;
    }

    public CharacterWeapon? Weapon { get; }
    
}