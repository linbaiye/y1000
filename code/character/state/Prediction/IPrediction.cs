using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.networking.message;
using y1000.code.networking.message.character;

namespace y1000.code.character.state.Prediction
{
    public interface IPrediction
    {
        IInput Input { get; }

        bool SyncWith(InputResponseMessage message);
    }
}