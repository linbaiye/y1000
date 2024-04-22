using y1000.code.networking.message;
using y1000.Source.Input;

namespace y1000.Source.Character.State.Prediction
{
    public abstract class AbstractPrediction : IPrediction
    {

        private readonly IPredictableInput _input;

        private readonly bool _clearPrevious;
        
        protected AbstractPrediction(IPredictableInput input) : this(input, false)
        {
        }

        protected AbstractPrediction(IPredictableInput input, bool clearPrevious)
        {
            _clearPrevious = clearPrevious;
            _input = input;
        }

        public IPredictableInput Input => _input;

        public bool ClearPrevious => _clearPrevious;

        public abstract bool SyncWith(InputResponseMessage message);
    }
}