using System;
using NLog;
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


        public void Clear()
        {
            _predictions.Clear();
        }

        public bool Reconcile(IPredictableResponse message)
        {
            var predicted = false;
            while (!_predictions.IsEmpty)
            {
                IPrediction prediction = _predictions.Front();
                _predictions.PopFront();
                if (prediction.Input.Sequence == message.Sequence)
                {
                    predicted = prediction.Predicted(message);
                    break;
                }
            }
            if (!predicted)
            {
                _predictions.Clear();
            }
            return predicted;
        }
    }
}