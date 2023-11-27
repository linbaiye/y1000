using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.entity.equipment.weapon
{
    public interface IWeapon: IEquipment
    {
        bool IsRange {get;}
    }
}