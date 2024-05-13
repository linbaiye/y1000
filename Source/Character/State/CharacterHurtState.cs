using y1000.Source.Player;

namespace y1000.Source.Character.State;

public sealed class CharacterHurtState : ICharacterState
{
    private readonly PlayerHurtState _playerHurtState;

    public CharacterHurtState(PlayerHurtState playerHurtState)
    {
        _playerHurtState = playerHurtState;
    }

    public void OnWrappedPlayerAnimationFinished(Character character)
    {
        character.ChangeState(CharacterCooldownState.Cooldown(character.IsMale));
    }

    public IPlayerState WrappedState => _playerHurtState;

    public static CharacterHurtState Hurt()
    {
        return new CharacterHurtState(PlayerHurtState.Hurt());
    }
}