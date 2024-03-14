using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code;
using y1000.code.character.state;
using y1000.code.character.state.Prediction;
using y1000.code.networking.message.character;

namespace y1000.Source.Character.State
{
    public class IdlePrediction : AbstractPrediction
    {

        private readonly Vector2I _coordinate;

        private readonly Direction _direction;


        public IdlePrediction(IInput input, Vector2I coordinate) : base(input)
        {
            _coordinate = coordinate;
        }

        public override bool SyncWith(ICharacterMessage message)
        {
            return true;
        }
    }
}