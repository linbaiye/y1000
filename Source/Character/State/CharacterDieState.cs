using y1000.Source.Creature;
using y1000.Source.Input;
using y1000.Source.Player;
using ILogger = NLog.ILogger;

namespace y1000.Source.Character.State;

public class CharacterDieState : ICharacterState
{
    public CharacterDieState(IPlayerState wrappedState)
    {
        WrappedState = wrappedState;
    }

    public void OnWrappedPlayerAnimationFinished(CharacterImpl character)
    {
    }

    public IPlayerState WrappedState { get; }

    public static CharacterDieState Die()
    {
        var st = IPlayerState.NonHurtState(CreatureState.DIE);
        return new CharacterDieState(st);
    }
}