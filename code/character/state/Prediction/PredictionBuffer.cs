using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.networking.message.character;

namespace y1000.code.character.state.Prediction
{
    public class PredictionManager
    {
        private readonly CircularBuffer<IPrediction> _predictions;


        public PredictionManager()
        {
            _predictions = new CircularBuffer<IPrediction>(200);
        }

        public void Save(IPrediction prediction)
        {
            if (_predictions.IsFull)
            {
                throw new Exception();
            }
            _predictions.PushBack(prediction);
        }


        public bool Reconcile(ICharacterMessage message)
        {
            while (!_predictions.IsEmpty)
            {
                IPrediction prediction = _predictions.Front();
                if (prediction.Input.Sequence <= message.InputSeqeunce)
                {
                    _predictions.PopFront();
                    if (prediction.SyncWith(message))
                    {
                        return true;
                    }
                }
                else
                {
                    // Probably a stale message.
                    return true;
                }
            }
            return false;
        }
    }
}