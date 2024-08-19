using y1000.code.networking.message;
using y1000.Source.Input;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character.State.Prediction
{
    public interface IPrediction
    {
        IPredictableInput Input { get; }

        bool ClearPrevious { get; }

        /// <summary>
        /// Did we predict the input correct?
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        bool Predicted(IPredictableResponse response);
    }
}