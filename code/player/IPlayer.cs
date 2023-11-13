using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;

namespace y1000.code.player
{
    public interface IPlayer : ICreature
    {
        void Sit();

        void Bow();

        bool IsMale();

        OffsetTexture ChestTexture { get; }
    }
}