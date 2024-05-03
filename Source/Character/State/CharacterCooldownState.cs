using y1000.Source.Player;

namespace y1000.Source.Character.State;

public class CharacterCooldownState : ICharacterState
{
    private readonly PlayerCooldownState _state;

    public CharacterCooldownState(PlayerCooldownState state)
    {
        _state = state;
        //_target = target;
    }
    
    public void OnWrappedPlayerAnimationFinished(Character character)
    {
        //character.AttackKungFu?.Attack(character, InputFactory.CreateAttack());
        //character.ChangeState(CharacterAttackState.Quanfa());
        //character.ChangeState(Cooldown(character.IsMale, _target, _millisPerSprite));
    }
    
    public IPlayerState WrappedState => _state;

    public static CharacterCooldownState Cooldown(bool male)
    {
        var playerCooldownState = PlayerCooldownState.Cooldown(male);
        return new CharacterCooldownState(playerCooldownState);
    }

}