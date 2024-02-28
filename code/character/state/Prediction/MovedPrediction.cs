using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.networking.message;
using y1000.code.networking.message.character;

namespace y1000.code.character.state.Prediction
{
    public class MovedPrediction : AbstractPrediction
    {
        private readonly Point _newCoordinate;

        public MovedPrediction(IInput input, Point nextCoordinate) : base(input)
        {
            _newCoordinate = nextCoordinate;
        }

        public override bool SyncWith(ICharacterMessage message)
        {
            if (message.InputSeqeunce != Input.Sequence)
            {
                return false;
            }
            return message is CharacterMovingMessage movingMessage &&
                movingMessage.NewCoordinate.Equals(_newCoordinate);
        }
    }
}