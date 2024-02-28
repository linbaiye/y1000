using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.networking.message.character
{
    public class CharacterMovingMessage : AbstractCharacterMessage
    {
        private readonly Point _newCoordinate;

        public CharacterMovingMessage(long sequence, Point newCoordinate) : base(sequence)
        {
            _newCoordinate = newCoordinate;
        }

        public Point NewCoordinate => _newCoordinate;
    }
}