using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.character.state.input
{
    public class InputFactory
    {
        private static int sequence = 0;

        public static MouseMoveInput CreateMouseMoveInput(Direction direction)
        {
            return new MouseMoveInput(sequence++, direction);
        }
    }
}