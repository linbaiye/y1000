using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.networking.message.character
{
    public class CharacterMovingMessage : AbstractCharacterMessage
    {
        private readonly Vector2I _newCoordinate;

        public CharacterMovingMessage(long sequence, Vector2I newCoordinate) : base(sequence)
        {
            _newCoordinate = newCoordinate;
        }

        public Vector2I NewCoordinate => _newCoordinate;
    }
}