using Google.Protobuf.WellKnownTypes;
using y1000.code.networking.message;
using y1000.Source.Character.Event;
using y1000.Source.Input;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character.State.Prediction;

public class AttackPrediction : AbstractPrediction
{
    public AttackPrediction(IPredictableInput input) : base(input, false)
    {
    }

    public override bool Predicted(IPredictableResponse response)
    {
        return response is CharacterAttackEventResponse { Accepted: true };
    }
}