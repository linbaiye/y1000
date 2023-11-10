using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.world;

namespace y1000.code.player
{
    public interface ICharater : ICreature
    {
        void Move(GameMap gameMap, IEnumerable<ICreature> creatures);
    }
}