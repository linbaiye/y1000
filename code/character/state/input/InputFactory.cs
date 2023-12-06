using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.character.state.input
{
    public class InputFactory
    {
        private static long sequence = 0;

        public static MouseRightClick CreateMouseMoveInput(Direction direction)
        {
            return new MouseRightClick(sequence++, direction);
        }
    }
}