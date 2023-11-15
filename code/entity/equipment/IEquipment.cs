using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.entity.equipment
{
    public interface IEquipment: IEntity
    {
        string SpriteName { get; }

        string SpriteBasePath { get; }
    }
}