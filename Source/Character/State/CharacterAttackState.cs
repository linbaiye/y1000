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
        character.ChangeState(CharacterIdleState.Create(character.IsMale));
    }

    public IPlayerState WrappedState => _playerAttackState;
    

    public static CharacterAttackState Create(bool male, IEntity target)
    {
        var playerAttackState = PlayerAttackState.Create(male, 0);
        return new CharacterAttackState(playerAttackState, target);
    }
}