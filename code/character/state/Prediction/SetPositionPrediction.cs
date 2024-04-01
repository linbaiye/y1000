using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.networking.message;

namespace y1000.code.character.state.Prediction
{
    public class SetPositionPrediction : AbstractPositionPrediction<SetPositionMessage>
    {
        public SetPositionPrediction(IInput input, Vector2I currentCoordinate, Direction direction) : base(input, currentCoordinate, direction)
        {
        }
    }
}