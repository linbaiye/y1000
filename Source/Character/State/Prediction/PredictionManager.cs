using System;
using NLog;
using y1000.code.networking.message;

namespace y1000.Source.Character.State.Prediction
{
    public class PredictionManager
    {
        private readonly CircularBuffer<IPrediction> _predictions;
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public PredictionManager()
        {
            _predictions = new CircularBuffer<IPrediction>(200);
        }

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


        public bool Reconcile(InputResponseMessage message)
        {
            while (!_predictions.IsEmpty)
            {
                IPrediction prediction = _predictions.Front();
                if (prediction.Input.Sequence == message.Sequence)
                {
                    _predictions.PopFront();
                    if (prediction.SyncWith(message))
                    {
                        return true;
                    }
                }
                else if (prediction.Input.Sequence < message.Sequence)
                {
                    logger.Info("Lost input response for input {0}.", prediction.Input);
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