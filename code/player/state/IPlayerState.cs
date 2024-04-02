using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;
using y1000.code.player.skill;

namespace y1000.code.player.state
{
    public interface IPlayerState : ICreatureState
    {
        OffsetTexture ChestTexture(int animationSpriteNumber, ChestArmor armor);

        OffsetTexture HatTexture(int animationSpriteNumber, Hat hat);

        OffsetTexture TrousersTexture(int animationSpriteNumber, Trousers trousers);

        OffsetTexture WeaponTexture(int animationSpriteNumber, IWeapon weapon);

        void Bow() {}

        void Sit() {}

        /*
        * Return true if bufa can be pressed.
        */
        bool PressBufa(IBufa bufa);


        

    }
}