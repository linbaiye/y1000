using y1000.code.networking.message;
using y1000.Source.Input;

namespace y1000.Source.Character.State.Prediction
{
    public interface IPrediction
    {
        IPredictableInput Input { get; }

        bool ClearPrevious { get; }

        bool SyncWith(InputResponseMessage message);
    }
}