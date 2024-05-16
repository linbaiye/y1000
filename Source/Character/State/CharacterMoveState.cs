using y1000.Source.Creature;
using y1000.Source.Input;
using y1000.Source.KungFu.Foot;
using y1000.Source.Player;

namespace y1000.Source.Character.State
{
    public class CharacterMoveState : ICharacterState
    {
        private CharacterMoveState(IPlayerState wrappedState)
        {
            WrappedState = wrappedState;
        }

        public static CharacterMoveState Move(IFootKungFu? magic, AbstractRightClickInput rightClickInput)
        {
            if (magic != null)
            {
                return magic.CanFly ? Fly(rightClickInput) : Run(rightClickInput);
            }
            return Walk(rightClickInput);
        }

        public void OnWrappedPlayerAnimationFinished(Character character)
        {
            if (WrappedState.State == CreatureState.ENFIGHT_WALK)
            {
                character.ChangeState(CharacterCooldownState.Cooldown());
            }
            else
            {
                character.ChangeState(CharacterIdleState.Idle());
            }
        }

        private static CharacterMoveState Walk(AbstractRightClickInput input)
        {
            return new CharacterMoveState(PlayerMoveState.WalkTowards(input.Direction));
        }

        private static CharacterMoveState Run(AbstractRightClickInput input)
        {
            return new CharacterMoveState(PlayerMoveState.RunTowards(input.Direction));
        }

        private static CharacterMoveState Fly(AbstractRightClickInput input)
        {
            return new CharacterMoveState(PlayerMoveState.FlyTowards(input.Direction));
        }

        public static CharacterMoveState EnfightWalk(AbstractRightClickInput input)
        {
            return new CharacterMoveState(PlayerMoveState.FlyTowards(input.Direction));
        }

        public IPlayerState WrappedState { get; }
    }
}