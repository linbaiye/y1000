using y1000.Source.Character.Event;
using y1000.Source.Input;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character.State.Prediction;

public class AttackPrediction : AbstractPrediction
{

    private readonly CharacterImpl _character;
    
    public AttackPrediction(IPredictableInput input, CharacterImpl character) : base(input, false)
    {
        _character = character;
    }

    public override bool Predicted(IPredictableResponse response)
    {
        if (response is CharacterAttackEventResponse attackEventResponse)
        {
            _character.HandleAttackResponse(attackEventResponse.BackToState);
            return true;
        }
        return false;
    }
}