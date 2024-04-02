using y1000.code.networking.message;
using y1000.Source.Input;

namespace y1000.Source.Character.State.Prediction
{
    public abstract class AbstractPrediction : IPrediction
    {

        private readonly IInput _input;

        private readonly bool _clearPrevious;
        
        protected AbstractPrediction(IInput input) : this(input, false)
        {
        }

        protected AbstractPrediction(IInput input, bool clearPrevious)
        {
            _clearPrevious = clearPrevious;
            _input = input;
        }

        public IInput Input => _input;

        public bool ClearPrevious => _clearPrevious;

        public abstract bool SyncWith(InputResponseMessage message);
    }
}