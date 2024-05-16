using y1000.Source.Input;
using y1000.Source.Player;

namespace y1000.Source.Character.State
{
    public class CharacterIdleState : AbstractCharacterIdleState
    {
        private CharacterIdleState(IPlayerState wrappedState) : base(wrappedState)
        {
        }
        
        protected override ICharacterState MoveState(Character character, AbstractRightClickInput rightClickInput)
        {
            return CharacterMoveState.Move(character.FootMagic, rightClickInput);
        }

        public static CharacterIdleState Idle()
        {
            return Wrap(IPlayerState.Idle());
        }

        public static CharacterIdleState Wrap(IPlayerState idleState)
        {
            return new CharacterIdleState(idleState);
        }
    }
}