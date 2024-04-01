using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.networking.message;
using y1000.code.networking.message.character;

namespace y1000.code.character.state.Prediction
{
    public abstract class AbstractPrediction : IPrediction
    {

        private readonly IInput _input;

        protected AbstractPrediction(IInput input)
        {
            _input = input;
        }

        public IInput Input => _input;

        public abstract bool SyncWith(InputResponseMessage message);
    }
}