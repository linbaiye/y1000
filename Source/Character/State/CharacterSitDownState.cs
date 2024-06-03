using NLog;
using y1000.Source.Creature;
using y1000.Source.Input;
using y1000.Source.Player;
using ILogger = NLog.ILogger;

namespace y1000.Source.Character.State;

public class CharacterSitDownState : ICharacterState
{
    private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
    private CharacterSitDownState(IPlayerState wrappedState)
    {
        WrappedState = wrappedState;
    }

    public IPlayerState WrappedState { get; }


    public static CharacterSitDownState SitDown()
    {
        var nonHurtState = IPlayerState.NonHurtState(CreatureState.SIT);
        return new CharacterSitDownState(nonHurtState);
    }

    public bool CanStandUp()
    {
        return WrappedState.ElapsedMillis >= WrappedState.TotalMillis;
    }

    public bool CanHandle(IPredictableInput input)
    {
        return input is AttackInput;
    }

    public bool CanAttack()
    {
        return true;
    }
}