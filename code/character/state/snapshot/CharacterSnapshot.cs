using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.character.state.snapshot
{
    public class CharacterSnapshot
    {
        public CreatureState State {get; set;}

        public Point Coordinate {get; set;}

        public Direction Direction {get; set;}
    }
}