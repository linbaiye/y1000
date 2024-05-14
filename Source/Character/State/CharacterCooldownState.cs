using y1000.Source.Player;

namespace y1000.Source.Character.State;

public class CharacterCooldownState : ICharacterState
{
    private CharacterCooldownState(IPlayerState state)
    {
        WrappedState = state;
    }
    
    public void OnWrappedPlayerAnimationFinished(Character character)
    {
        character.ChangeState(Cooldown());
    }
    
    public IPlayerState WrappedState { get; }

    public static CharacterCooldownState Cooldown()
    {
        return new CharacterCooldownState(IPlayerState.Cooldown());
    }
}