using y1000.Source.Input;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character.State.Prediction
{
    public abstract class AbstractPrediction : IPrediction
    {
        protected AbstractPrediction(IPredictableInput input) : this(input, false)
        {
        }

        protected AbstractPrediction(IPredictableInput input, bool clearPrevious)
        {
            ClearPrevious = clearPrevious;
            Input = input;
        }

        public IPredictableInput Input { get; }

        public bool ClearPrevious { get; }
        
        public abstract bool Predicted(IPredictableResponse response);
    }
}