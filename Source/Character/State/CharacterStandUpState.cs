using y1000.Source.Creature;
using IPlayerState = y1000.Source.Player.IPlayerState;

namespace y1000.Source.Character.State;

public class CharacterStandUpState : ICharacterState
{
    private CharacterStandUpState(IPlayerState wrappedState)
    {
        WrappedState = wrappedState;
    }

    public IPlayerState WrappedState { get; }


    public void OnWrappedPlayerAnimationFinished(CharacterImpl character)
    {
        character.ChangeState(CharacterIdleState.Idle());
    }

    public static CharacterStandUpState StandUp()
    {
        var state = IPlayerState.NonHurtState(CreatureState.STANDUP);
        return new CharacterStandUpState(state);
    }
}