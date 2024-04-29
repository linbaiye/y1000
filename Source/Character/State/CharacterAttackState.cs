using y1000.Source.Entity;
using y1000.Source.Input;
using y1000.Source.Player;

namespace y1000.Source.Character.State;

public class CharacterAttackState : ICharacterState
{
    private readonly PlayerAttackState _playerAttackState;

    private readonly IEntity _target;

    public CharacterAttackState(PlayerAttackState playerAttackState, IEntity target)
    {
        _playerAttackState = playerAttackState;
        _target = target;
    }

    public void OnWrappedPlayerAnimationFinished(Character character)
    {
        character.ChangeState(CharacterCooldownState.Cooldown(character.IsMale, _target, 500));
    }

    public IPlayerState WrappedState => _playerAttackState;

    public static CharacterAttackState Quanfa(bool male, IEntity target, bool below50)
    {
        var playerAttackState = PlayerAttackState.QuanfaAttack(male, below50, below50 ? 90 : 75);
        return new CharacterAttackState(playerAttackState, target);
    }
}