using System;
using y1000.Source.Item;

namespace y1000.Source.Character.Event;

public class WeaponChangedEvent : EventArgs
{
    public WeaponChangedEvent(PlayerWeapon? weapon)
    {
        Weapon = weapon;
    }

    public PlayerWeapon? Weapon { get; }
    
}