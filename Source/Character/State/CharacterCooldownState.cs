using y1000.Source.Entity;
using y1000.Source.Input;
using y1000.Source.Player;

namespace y1000.Source.Character.State;

public class CharacterCooldownState : ICharacterState
{
    private readonly PlayerCooldownState _state;

    private readonly IEntity _target;

    private readonly int _millisPerSprite;
    
    public CharacterCooldownState(PlayerCooldownState state, IEntity target, int millisPerSprite)
    {
        _state = state;
        _target = target;
        _millisPerSprite = millisPerSprite;
    }
    
    public void OnWrappedPlayerAnimationFinished(Character character)
    {
        character.AttackKungFu?.Attack(character, InputFactory.CreateAttack(_target));
        //character.ChangeState(CharacterAttackState.Quanfa());
        //character.ChangeState(Cooldown(character.IsMale, _target, _millisPerSprite));
    }
    
    public IPlayerState WrappedState => _state;

    public static CharacterCooldownState Cooldown(bool male, IEntity target, int millisPerSprite)
    {
        var playerCooldownState = PlayerCooldownState.Cooldown(male, millisPerSprite);
        return new CharacterCooldownState(playerCooldownState, target, millisPerSprite);
    }
}