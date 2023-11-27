using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;
using y1000.code.player.skill;

namespace y1000.code.player
{
    public interface IPlayer : ICreature
    {
        void Sit();

        void Bow();

        bool IsMale();

        OffsetTexture? ChestTexture { get; }

        ChestArmor? ChestArmor { get; set; }

        OffsetTexture? HatTexture { get; }

        Hat? Hat { get; set; }

        Trousers? Trousers { get; set; }

        OffsetTexture? TrousersTexture { get; }

        IWeapon? Weapon {get; set;}

        OffsetTexture? WeaponTexture { get; }

        IBufa? Bufa {get;}

        void PressBufa(IBufa bufa);
    }
}