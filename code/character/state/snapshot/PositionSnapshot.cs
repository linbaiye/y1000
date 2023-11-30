using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.character.state.input;
using y1000.code.networking;
using y1000.code.networking.message;

namespace y1000.code.character.state.snapshot
{
    public class PositionSnapshot : IStateSnapshot
    {
        private readonly Point currentPoint;

        private readonly Direction currentDirection;

        public bool Match(IInput input, IUpdateStateMessage message)
        {
            if (input is not MouseMoveInput mouseMoveInput)
            {
                return false;
            }
            if (message is not MoveStateMessage moveStateMessage)
            {
                return false;
            }
        }
    }
}