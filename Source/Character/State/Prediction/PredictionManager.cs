using System;
using NLog;
using y1000.code.networking.message;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character.State.Prediction
{
    public class PredictionManager
    {
        private readonly CircularBuffer<IPrediction> _predictions = new(200);
        
        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

        public void Save(IPrediction prediction)
        {
            if (prediction.ClearPrevious)
                _predictions.Clear();
            if (_predictions.IsFull)
            {
                throw new Exception();
            }
            _predictions.PushBack(prediction);
        }


        public bool Reconcile(IPredictableResponse message)
        {
            while (!_predictions.IsEmpty)
            {
                IPrediction prediction = _predictions.Front();
                if (prediction.Input.Sequence == message.Sequence)
                {
                    _predictions.PopFront();
                    if (prediction.Predicted(message))
                    {
                        return true;
                    }
                }
                else if (prediction.Input.Sequence < message.Sequence)
                {
                    LOGGER.Info("Lost input response for input {0}.", prediction.Input);
                    _predictions.PopFront();
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